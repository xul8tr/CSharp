// smartcardalarm.cpp : Defines the entry point for the application.
// http://zetcode.com/gui/winapi/menus/
// Filehandling
// https://msdn.microsoft.com/en-us/library/windows/desktop/bb540534%28v=vs.85%29.aspx
// http://en.wikipedia.org/wiki/Piano_key_frequencies

#include "stdafx.h"
#include "SmartCardAlarm.h"
#include "Wtsapi32.h"
#include "Winscard.h"
#include "shellapi.h"
#include "shlobj.h"
#include <MMDeviceAPI.h>
#include <endpointvolume.h>
#include <shlwapi.h>
#include <string>
#pragma comment(lib,"shlwapi.lib")

#define MAX_LOADSTRING					64
#define ID_TRAY_APP_ICON                5000
#define ID_TRAY_EXIT_CONTEXT_MENU_ITEM  3000
#define ID_TRAY_BOCI_CONTEXT_MENU_ITEM  3001
#define ID_TRAY_DEF_CONTEXT_MENU_ITEM   3002
#define ID_TRAY_TEST_CONTEXT_MENU_ITEM  3003
#define ID_TRAY_LONDON_CONTEXT_MENU_ITEM  3004
#define ID_TRAY_BREKI_CONTEXT_MENU_ITEM  3005
#define WM_TRAYICON						( WM_USER + 1 )
#define BUFFERSIZE						8

// Global Variables:
HINSTANCE hInst;								// current instance
TCHAR szTitle[MAX_LOADSTRING];					// The title bar text
TCHAR szWindowClass[MAX_LOADSTRING];			// the main window class name
HMENU g_menu ;
NOTIFYICONDATA g_notifyIconData ;
int bociNotes[] =    {2093,2637,2093,2637,3136,3136,10  ,2093,2637,2093,2637,3136,3136,10  ,4186,3951,3520,3136,2794,3520,10  ,3136,2794,2637,2349,2093,2093};
int bociDuration[] = {300 ,300 ,300 ,300, 750 ,300, 300 ,300 ,300 ,300 ,300 ,750 ,300 ,300 ,300 ,300 ,300 ,300 ,750 ,300 ,300 ,300 ,300 ,300 ,300 ,750 ,750 };
int defaultBeepNotes[] =	{2000,3000,2000,3000,2000,3000,2000,3000,2000,3000,2000,3000,2000,3000,2000,3000,2000,3000};
int defaultBeepDuration[] = {1000,1000,1000,1000,1000,1000,1000,1000,1000,1000,1000,1000,1000,1000,1000,1000,1000,1000};
int londonNotes[] =		{3136,3520,3136,2794,2637,2794,3136,2349,2637,2794,2637,2794,3136,3136,3520,3136,2794,2637,2794,3136,2349,3136,2637,2093};
int londonDuration[] =	{450 ,150 ,300 ,300 ,300 ,300 ,600 ,300 ,300 ,600 ,300 ,300 ,600 ,450 ,150 ,300 ,300 ,300 ,300 ,600 ,600 ,600 ,300 ,750};
int brekiNotes[] =		{3136,3136,3136,3136,	3136,3520,3951,3136,	3520,3520,3520,3520,	4699,4186,3951,3520,	3136,3136,2349,	3136,3136,2349,	2637,4186,3951,3520,	3136,3136,3136};
int brekiDuration[] =	{300 ,300 ,300 ,270 ,	300 ,300 ,300 ,300 ,	300 ,300 ,300 ,300 ,	300 ,300 ,300 ,300 ,	300 ,300 ,600 ,	300 ,300 ,600 ,	300 ,300 ,300 ,300 ,	300 ,300 ,600 };
int* notes;
int* duration;
int soundLocator = 0;
int length;
TCHAR szPath[MAX_PATH] = _T("");

struct MutedAndVol
{
	BOOL Muted;
	float Volume;
};

void SelectBoci();
void SelectDefault();
void SelectLondon();
void SelectBreki();
void ClearMenuItemChecks();
MutedAndVol HandleMixer(MutedAndVol);
void InitNotifyIconData(HWND);
void InitializeSoundTheme();
int checkCard();
void PlayNext();
void CreateAndFillPopupMenu();
void HandlePopupMenuClick(HWND);

BOOL InitInstance(HINSTANCE hInstance, int nCmdShow)
{
	hInst = hInstance; // Store instance handle in our global variable
	HWND hWnd = CreateWindow(szWindowClass, szTitle, WS_OVERLAPPEDWINDOW, CW_USEDEFAULT, 0, CW_USEDEFAULT, 0, NULL, NULL, hInstance, NULL);
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
	switch (message)
	{
	case WM_CREATE:            
		CreateAndFillPopupMenu();
		InitializeSoundTheme();
		break;

	case WM_WTSSESSION_CHANGE:
		if ( wParam == WTS_SESSION_LOCK || wParam == WTS_SESSION_LOGOFF ||wParam == WTS_CONSOLE_DISCONNECT) 
		{
			checkCard();
		}

		break;
	case WM_DESTROY:
		PostQuitMessage(0);
		break;

	case WM_TRAYICON:
		{   
			if (lParam == WM_RBUTTONDOWN) 
			{
				HandlePopupMenuClick(hWnd);
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

int APIENTRY _tWinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPTSTR lpCmdLine, int nCmdShow)
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

void CreateAndFillPopupMenu()
{
	// create the menu once.
	// oddly, you don't seem to have to explicitly attach
	// the menu to the HWND at all.  This seems so ODD.
	g_menu = CreatePopupMenu();	
	AppendMenu(g_menu, MF_STRING, ID_TRAY_BOCI_CONTEXT_MENU_ITEM,  TEXT( "Boci" ) );
	AppendMenu(g_menu, MF_STRING, ID_TRAY_LONDON_CONTEXT_MENU_ITEM,  TEXT( "London" ) );
	AppendMenu(g_menu, MF_STRING, ID_TRAY_BREKI_CONTEXT_MENU_ITEM,  TEXT( "Breki" ) );
	CheckMenuItem(g_menu, ID_TRAY_BOCI_CONTEXT_MENU_ITEM, MF_CHECKED);
	AppendMenu(g_menu, MF_STRING, ID_TRAY_DEF_CONTEXT_MENU_ITEM,  TEXT( "Default" ) );
	AppendMenu(g_menu, MF_STRING, ID_TRAY_TEST_CONTEXT_MENU_ITEM,  TEXT( "Próba! :)" ) );
	AppendMenu(g_menu, MF_STRING, ID_TRAY_EXIT_CONTEXT_MENU_ITEM,  TEXT( "Exit" ) );
}

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
	wcsncpy_s(g_notifyIconData.szTip, TEXT("Smartcard Alarm is active"), MAX_LOADSTRING);
}

void HandlePopupMenuClick(HWND hWnd)
{
	// Get current mouse position.
	POINT curPoint ;
	GetCursorPos( &curPoint );
	// should SetForegroundWindow according
	// to original poster so the popup shows on top
	// SetForegroundWindow(hWnd);
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

	if (clicked == ID_TRAY_EXIT_CONTEXT_MENU_ITEM)
	{
		// quit the application.
		PostQuitMessage( 0 ) ;
	}
	if (clicked == ID_TRAY_BOCI_CONTEXT_MENU_ITEM)
	{
		SelectBoci();
	}
	if (clicked == ID_TRAY_LONDON_CONTEXT_MENU_ITEM)
	{
		SelectLondon();
	}
	if (clicked == ID_TRAY_BREKI_CONTEXT_MENU_ITEM)
	{
		SelectBreki();
	}
	if (clicked == ID_TRAY_DEF_CONTEXT_MENU_ITEM)
	{
		SelectDefault();
	}
	if (clicked == ID_TRAY_TEST_CONTEXT_MENU_ITEM)
	{
		checkCard();
	}
}

int checkCard()
{
	SCARDCONTEXT    hSC = 0;
	LONG            lReturn = -1;
	LPTSTR          pmszReaders = 0;
	DWORD           cch = SCARD_AUTOALLOCATE;
	SCARDHANDLE     hCardHandle;
	DWORD           dwAP;

	// Establish the context.
	lReturn = SCardEstablishContext(SCARD_SCOPE_USER, NULL, NULL, &hSC);
	if ( SCARD_S_SUCCESS != lReturn ) return -1;

	// Retrieve the list of the readers.
	lReturn = SCardListReaders(hSC, NULL, (LPTSTR)&pmszReaders, &cch );
	if ( lReturn == SCARD_S_SUCCESS )
	{ 
		//we have cards
		MutedAndVol mutedAndVol = MutedAndVol();
		mutedAndVol.Muted = false;
		mutedAndVol.Volume = 1.0;
		mutedAndVol= HandleMixer(mutedAndVol);
		int i = 0;
		bool bInserted = true;            
		while (bInserted && i<length)
		{
			bInserted = (SCARD_S_SUCCESS == SCardConnect( hSC, pmszReaders, SCARD_SHARE_SHARED, SCARD_PROTOCOL_T0 | SCARD_PROTOCOL_T1, &hCardHandle, &dwAP));
			SCardDisconnect(hCardHandle, SCARD_LEAVE_CARD);
			if (bInserted)
			{
				i++;
				PlayNext();                
			}
		}

		SCardDisconnect(hCardHandle, SCARD_LEAVE_CARD);
		soundLocator = 0;
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

LPTSTR GetFilePath ()
{
	if (_tcscmp(szPath,_T("")) == 0)
	{
		if (SUCCEEDED(SHGetFolderPath(NULL, CSIDL_APPDATA, NULL, 0, szPath)))
		{		
			PathAppend( szPath, _T("\\evosoft\\"));
			DWORD ftyp = GetFileAttributes(szPath);
			if (ftyp == INVALID_FILE_ATTRIBUTES)
			{
				if (!CreateDirectory(szPath, NULL))
				{
					_tcscpy_s(szPath, _T(""));
					return _T("");
				}
			}

			PathAppend( szPath, _T("\\SmartCardAlarm\\"));
			ftyp = GetFileAttributes(szPath);
			if (ftyp == INVALID_FILE_ATTRIBUTES)
			{
				if (!CreateDirectory(szPath, NULL))
				{
					_tcscpy_s(szPath, _T(""));
					return _T("");
				}
			}

			PathAppend( szPath, _T("settings.set"));
		}
	}

	return szPath;
}

void SaveSettings(char DataBuffer[])
{
	LPTSTR path = GetFilePath ();
	HANDLE hFile = CreateFile(path,    // name of the write
		GENERIC_WRITE,          // open for writing
		0,                      // do not share
		NULL,                   // default security
		CREATE_ALWAYS,          // create new file
		FILE_ATTRIBUTE_NORMAL,  // normal file
		NULL);                  // no attr. template
	if (hFile != INVALID_HANDLE_VALUE) 
	{ 
		DWORD dwBytesToWrite = (DWORD)strlen(DataBuffer);
		DWORD dwBytesWritten = 0;
		BOOL bErrorFlag = WriteFile( 
			hFile,           // open file handle
			DataBuffer,      // start of data to write
			dwBytesToWrite,  // number of bytes to write
			&dwBytesWritten, // number of bytes that were written
			NULL);            // no overlapped structure

		if (FALSE == bErrorFlag)
		{
			Beep (2000,500);
			MessageBox(NULL,_T("Unable to save configuration.\nPlease restart the application.\nError: Cannot write settings file."),_T("SmartCard Alarm"),0);
		}
	}
	else
	{
		Beep (2000,500);
		MessageBox(NULL,_T("Unable to save configuration.\nPlease restart the application.\nError: Path does not exist or file is read only."),_T("SmartCard Alarm"),0);
	}

	CloseHandle(hFile);
}

void ReadSettings()
{    
	HANDLE hFile; 
	DWORD  dwBytesRead = 0;
	// Known as simple chars, no wide chars are used.
	char ReadBuffer[BUFFERSIZE] = {0};
	OVERLAPPED ol = {0};
	BOOL bReadFileResult;
	LPTSTR path = GetFilePath ();
	hFile = CreateFile(path,          // file to open
		GENERIC_READ,          // open for reading
		FILE_SHARE_READ,       // share for reading
		NULL,                  // default security
		OPEN_EXISTING,         // existing file only
		FILE_ATTRIBUTE_NORMAL, // normal file
		NULL);                 // no attr. template

	if (hFile != INVALID_HANDLE_VALUE) 
	{ 
		bReadFileResult = ReadFile(hFile, ReadBuffer, BUFFERSIZE-1, &dwBytesRead, &ol);
		CloseHandle(hFile);
		if(bReadFileResult)
		{
			// Known as simple chars, no wide chars are used.
			if (strcmp(ReadBuffer, "BOCI")== 0)
			{
				SelectBoci();
			}
			else if (strcmp(ReadBuffer, "LONDON")== 0)
			{
				SelectLondon();
			}

			else if (strcmp(ReadBuffer, "BREKI")== 0)
			{
				SelectBreki();
			}
			else
			{
				SelectDefault();
			}
		}
		else
		{
			SelectDefault();
		}
	}
	else
	{
		SelectDefault();
	}	
}

void InitializeSoundTheme()
{
	ReadSettings();
}

void ClearMenuItemChecks()
{
	CheckMenuItem(g_menu, ID_TRAY_DEF_CONTEXT_MENU_ITEM, MF_UNCHECKED);
	CheckMenuItem(g_menu, ID_TRAY_BOCI_CONTEXT_MENU_ITEM, MF_UNCHECKED);
	CheckMenuItem(g_menu, ID_TRAY_LONDON_CONTEXT_MENU_ITEM, MF_UNCHECKED);
	CheckMenuItem(g_menu, ID_TRAY_BREKI_CONTEXT_MENU_ITEM, MF_UNCHECKED);

}

void SelectBoci()
{
	ClearMenuItemChecks();
	CheckMenuItem(g_menu, ID_TRAY_BOCI_CONTEXT_MENU_ITEM, MF_CHECKED);
	notes = bociNotes;
	duration = bociDuration;
	length = sizeof(bociNotes)/sizeof(bociNotes[0]);
	SaveSettings("BOCI");
}

void SelectLondon()
{
	ClearMenuItemChecks();
	CheckMenuItem(g_menu, ID_TRAY_LONDON_CONTEXT_MENU_ITEM, MF_CHECKED);
	notes = londonNotes;
	duration = londonDuration;
	length = sizeof(londonNotes)/sizeof(londonNotes[0]);
	SaveSettings("LONDON");
}

void SelectBreki()
{
	ClearMenuItemChecks();
	CheckMenuItem(g_menu, ID_TRAY_BREKI_CONTEXT_MENU_ITEM, MF_CHECKED);
	notes = brekiNotes;
	duration = brekiDuration;
	length = sizeof(brekiNotes)/sizeof(brekiNotes[0]);
	SaveSettings("BREKI");
}

void SelectDefault()
{
	ClearMenuItemChecks();
	CheckMenuItem(g_menu, ID_TRAY_DEF_CONTEXT_MENU_ITEM, MF_CHECKED);
	notes = defaultBeepNotes;
	duration = defaultBeepDuration;
	length = sizeof(defaultBeepNotes)/sizeof(defaultBeepNotes[0]);
	SaveSettings("DEFAULT");
}

void PlayNext()
{
	Beep(notes[soundLocator],duration[soundLocator]);
	soundLocator++;
}

