// smartcardalarm.cpp : Defines the entry point for the application.
//

#include "stdafx.h"
#include "SmartCardAlarm.h"
#include "Wtsapi32.h"
#include "Winscard.h"
#include "shellapi.h"
#include <MMDeviceAPI.h>
#include <endpointvolume.h>

#define MAX_LOADSTRING					64
#define ID_TRAY_APP_ICON                5000
#define ID_TRAY_EXIT_CONTEXT_MENU_ITEM  3000
#define WM_TRAYICON						( WM_USER + 1 )


// Global Variables:
HINSTANCE hInst;								// current instance
TCHAR szTitle[MAX_LOADSTRING];					// The title bar text
TCHAR szWindowClass[MAX_LOADSTRING];			// the main window class name
HMENU g_menu ;
NOTIFYICONDATA g_notifyIconData ;

struct MutedAndVol
{
    BOOL Muted;
    float Volume;
};

void InitNotifyIconData(HWND hWnd)
{
  memset( &g_notifyIconData, 0, sizeof( NOTIFYICONDATA ) ) ;
  g_notifyIconData.cbSize = sizeof(NOTIFYICONDATA);
  
  g_notifyIconData.hWnd = hWnd;
  g_notifyIconData.uID = ID_TRAY_APP_ICON;
  // Set up flags.
  g_notifyIconData.uFlags = NIF_ICON | // promise that the hIcon member WILL BE A VALID ICON!!
    NIF_MESSAGE | // when someone clicks on the system tray icon,
    // we want a WM_ type message to be sent to our WNDPROC
    NIF_TIP;      // we're gonna provide a tooltip as well, son.

  g_notifyIconData.uCallbackMessage = WM_TRAYICON; //this message must be handled in hwnd's window procedure
  
  g_notifyIconData.hIcon = LoadIcon(hInst, MAKEINTRESOURCE(IDI_APP)) ; // The icon that will be shown in the systray.

  // set the tooltip text.  must be LESS THAN 64 chars
  wcsncpy_s(g_notifyIconData.szTip, TEXT("SmartcardAlarm is active"),MAX_LOADSTRING);
}

MutedAndVol HandleMixer(MutedAndVol mutedAndVol)
{
 
    HRESULT hr=NULL;
    CoInitialize(NULL);
    IMMDeviceEnumerator *deviceEnumerator = NULL;
    hr = CoCreateInstance(__uuidof(MMDeviceEnumerator), NULL, CLSCTX_INPROC_SERVER, __uuidof(IMMDeviceEnumerator), (LPVOID *)&deviceEnumerator);

    IMMDevice *defaultDevice = NULL;
    hr = deviceEnumerator->GetDefaultAudioEndpoint(eRender, eConsole, &defaultDevice);
    deviceEnumerator->Release();
    deviceEnumerator = NULL;
 
    IAudioEndpointVolume *endpointVolume = NULL;
    hr = defaultDevice->Activate(__uuidof(IAudioEndpointVolume), CLSCTX_INPROC_SERVER, NULL, (LPVOID *)&endpointVolume);
    defaultDevice->Release();
    defaultDevice = NULL;

    BOOL muted;
    float volume;
    endpointVolume->GetMute(&muted);
    endpointVolume->GetMasterVolumeLevelScalar(&volume);
    if (mutedAndVol.Muted != muted)
    {
        endpointVolume->SetMute(mutedAndVol.Muted, NULL);
        mutedAndVol.Muted = muted;
    }
    
    if (mutedAndVol.Volume != volume)
    {
        endpointVolume->SetMasterVolumeLevelScalar(mutedAndVol.Volume, NULL);
        mutedAndVol.Volume = volume;
    }

    endpointVolume->Release();
    endpointVolume = NULL;	
    CoUninitialize();
    return mutedAndVol;
}


int checkCard()
{
    SCARDCONTEXT    hSC = 0;
    LONG            lReturn = -1;
    LPTSTR          pmszReaders = 0;
    LPTSTR          pReader = 0;
    DWORD           cch = SCARD_AUTOALLOCATE;
    SCARDHANDLE     hCardHandle;
    DWORD           dwAP;

    // Establish the context.
    lReturn = SCardEstablishContext(SCARD_SCOPE_USER, NULL, NULL, &hSC);
    if ( SCARD_S_SUCCESS != lReturn ) return -1;
    
    // Retrieve the list the readers.
    lReturn = SCardListReaders(hSC, NULL, (LPTSTR)&pmszReaders, &cch );
    if ( lReturn == SCARD_S_SUCCESS )
    { //we have cards
        MutedAndVol mutedAndVol = MutedAndVol();
        mutedAndVol.Muted = false;
        mutedAndVol.Volume = 1.0;

        mutedAndVol= HandleMixer(mutedAndVol);
        pReader = pmszReaders;
        while ( '\0' != *pReader )
        {
            int i = 0;
            bool bInserted = true;            
            while (bInserted && i<10)
            {
                bInserted = SCARD_S_SUCCESS == SCardConnect( hSC, pReader, SCARD_SHARE_SHARED, SCARD_PROTOCOL_T0 | SCARD_PROTOCOL_T1, &hCardHandle,&dwAP );
                if (bInserted)
                {
                    ++i;			
                    Beep(2000,1000);
                    Beep(3000,1000);
                    SCardDisconnect(hCardHandle, SCARD_LEAVE_CARD);
                }
            }

            SCardDisconnect(hCardHandle, SCARD_LEAVE_CARD);
            // Advance to the next value.
            pReader = pReader + wcslen((wchar_t *)pReader) + 1;
        }
        // Free the memory.
        SCardFreeMemory( hSC, pmszReaders );
        if (mutedAndVol.Muted || mutedAndVol.Volume != 1.0)
        {
            HandleMixer(mutedAndVol);
        }
    }

    // Free the context.
    SCardReleaseContext(hSC);
    return 0;

}

BOOL InitInstance(HINSTANCE hInstance, int nCmdShow)
{
   HWND hWnd;

   hInst = hInstance; // Store instance handle in our global variable

   hWnd = CreateWindow(szWindowClass, szTitle, WS_OVERLAPPEDWINDOW,
      CW_USEDEFAULT, 0, CW_USEDEFAULT, 0, NULL, NULL, hInstance, NULL);

   if (!hWnd)
   {
      return FALSE;
   }

    UpdateWindow(hWnd);

    // Initialize the NOTIFYICONDATA structure once
    InitNotifyIconData(hWnd);

    // add the icon to the system tray
    Shell_NotifyIcon(NIM_ADD, &g_notifyIconData);

    WTSRegisterSessionNotification(hWnd, NOTIFY_FOR_THIS_SESSION);

    return TRUE;
}

LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
    PAINTSTRUCT ps;
    HDC hdc;

    switch (message)
    {
        case WM_CREATE:

            // create the menu once.
            // oddly, you don't seem to have to explicitly attach
            // the menu to the HWND at all.  This seems so ODD.
            g_menu = CreatePopupMenu();

            AppendMenu(g_menu, MF_STRING, ID_TRAY_EXIT_CONTEXT_MENU_ITEM,  TEXT( "Exit" ) );

            break;

        case WM_WTSSESSION_CHANGE:
            if ( wParam == WTS_SESSION_LOCK ||
                 wParam == WTS_SESSION_LOGOFF ||
                 wParam == WTS_CONSOLE_DISCONNECT
                ) 
            {
                checkCard();
            }

            break;

    case WM_PAINT:
        hdc = BeginPaint(hWnd, &ps);
        // TODO: Add any drawing code here...
        EndPaint(hWnd, &ps);
        break;
    case WM_DESTROY:
        PostQuitMessage(0);
        break;

  case WM_TRAYICON:
    {   
      if (lParam == WM_RBUTTONDOWN) 
      {
        // it gives the app a more responsive feel.  Some apps
        // DO use this trick as well.  Right clicks won't make
        // the icon disappear, so you don't get any annoying behavior
        // with this (try it out!)

        // Get current mouse position.
        POINT curPoint ;
        GetCursorPos( &curPoint ) ;
        
        // should SetForegroundWindow according
        // to original poster so the popup shows on top
        SetForegroundWindow(hWnd); 
      
        // TrackPopupMenu blocks the app until TrackPopupMenu returns
        UINT clicked = TrackPopupMenu(
          
          g_menu,
          TPM_RETURNCMD | TPM_NONOTIFY, // don't send me WM_COMMAND messages about this window, instead return the identifier of the clicked menu item
          curPoint.x,
          curPoint.y,
          0,
          hWnd,
          NULL

        );

        // Original poster's line of code.  Haven't deleted it,
        // but haven't seen a need for it.
        //SendMessage(hwnd, WM_NULL, 0, 0); // send benign message to window to make sure the menu goes away.
        if (clicked == ID_TRAY_EXIT_CONTEXT_MENU_ITEM)
        {
          // quit the application.
          PostQuitMessage( 0 ) ;
        }
      }
    }
    break;
    default:
        return DefWindowProc(hWnd, message, wParam, lParam);
    }
    return 0;
}

ATOM MyRegisterClass(HINSTANCE hInstance)
{
    WNDCLASSEX wcex;

    wcex.cbSize = sizeof(WNDCLASSEX);

    wcex.style			= CS_HREDRAW | CS_VREDRAW;
    wcex.lpfnWndProc	= WndProc;
    wcex.cbClsExtra		= 0;
    wcex.cbWndExtra		= 0;
    wcex.hInstance		= hInstance;
    wcex.hIcon			= LoadIcon(hInstance, MAKEINTRESOURCE(IDI_APP));
    wcex.hCursor		= LoadCursor(NULL, IDC_ARROW);
    wcex.hbrBackground	= (HBRUSH)(COLOR_WINDOW+1);
    wcex.lpszMenuName	= MAKEINTRESOURCE(IDI_APPLICATION);
    wcex.lpszClassName	= szWindowClass;
    wcex.hIconSm		= LoadIcon(wcex.hInstance, MAKEINTRESOURCE(IDI_APP));

    return RegisterClassEx(&wcex);
}



int APIENTRY _tWinMain(HINSTANCE hInstance,
                     HINSTANCE hPrevInstance,
                     LPTSTR    lpCmdLine,
                     int       nCmdShow)
{
    UNREFERENCED_PARAMETER(hPrevInstance);
    UNREFERENCED_PARAMETER(lpCmdLine);

    // TODO: Place code here.
    MSG msg;

    // Initialize global strings
    LoadString(hInstance, IDS_APP_TITLE, szTitle, MAX_LOADSTRING);
    LoadString(hInstance, IDC_SMARTCARDALARM, szWindowClass, MAX_LOADSTRING);
    MyRegisterClass(hInstance);

    // Perform application initialization:
    if (!InitInstance (hInstance, nCmdShow))
    {
        return FALSE;
    }
    // Main message loop:
    while (GetMessage(&msg, NULL, 0, 0))
    {
        TranslateMessage(&msg);
        DispatchMessage(&msg);
    }

    Shell_NotifyIcon(NIM_DELETE, &g_notifyIconData);
    return (int) msg.wParam;
}
