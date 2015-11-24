using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//#include <WinSock2.h>//dont move this for windows.h mess with winsock
//#include <stdlib.h> //or this for GLUT mess with exit
//#include <Windows.h>
//#include <HNWorld.h>
//#include <conio.h>

namespace _3DSessionIndependentAsyncClient
{
//    int main(int argc, char* argv[]) {
//    glutInit(&argc, argv);
//#ifdef _WIN32
//    HNWindowsObjectFactory::setAsDefaultFactory();
//    HNWindowsNetworkObjectFactory::setAsDefaultFactory();
//#endif
//    //HNWorld::getInstance()->setBackcolor(HNPointXYZW(0,0,0,0));
//    //HNAdithScene *scene = new HNAdithScene();
//    //HNWorld::getInstance()->addScene(scene);
//    //HNWorld::getInstance()->start();
//    cout << "Hello";
//    int count = 0;
//    while (1)
//    {
//        cout << "Making Call" << count << endl;
//        string msg = "This is message no " + to_string(count)+" from HUNA Instance NO 3";
//        cout << msg;
//        callSocket(msg);
//        count++;
//        _getch();		
//    }
//    _getch();
//}

//int callSocket(string pushMessage)
//{
//    // Initialise Winsock
//    WSADATA WsaDat;
//    if (WSAStartup(MAKEWORD(2, 2), &WsaDat) != 0)
//    {
//        std::cout << "Winsock error - Winsock initialization failed\r\n";
//        WSACleanup();
//        system("PAUSE");
//        return 0;
//    }

//    // Create our socket
//    SOCKET Socket = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
//    if (Socket == INVALID_SOCKET)
//    {
//        std::cout << "Winsock error - Socket creation Failed!\r\n";
//        WSACleanup();
//        system("PAUSE");
//        return 0;
//    }

//    // Resolve IP address for hostname
//    struct hostent *host;
//    if ((host = gethostbyname("localhost")) == NULL)
//    {
//        std::cout << "Failed to resolve hostname.\r\n";
//        WSACleanup();
//        system("PAUSE");
//        return 0;
//    }

//    // Setup our socket address structure
//    SOCKADDR_IN SockAddr;
//    SockAddr.sin_port = htons(11000);
//    SockAddr.sin_family = AF_INET;
//    SockAddr.sin_addr.s_addr = *((unsigned long*)host->h_addr);

//    // Attempt to connect to server
//    if (connect(Socket, (SOCKADDR*)(&SockAddr), sizeof(SockAddr)) != 0)
//    {
//        std::cout << "Failed to establish connection with server\r\n";
//        WSACleanup();
//        system("PAUSE");
//        return 0;
//    }
//    //else{
//    //	std::cout << "Connected with server\r\n";
//    //	int count = 0;

//    //	// Display message from server
//    //	char buffer[1000];
//    //	memset(buffer, 0, 999);
//    //	int inDataLength = recv(Socket, buffer, 1000, 0);
//    //	std::cout << buffer;
//    //}

//    // If iMode!=0, non-blocking mode is enabled.
//    u_long iMode = 1;
//    ioctlsocket(Socket, FIONBIO, &iMode);

//    std::string a = pushMessage+"<EOF>";
//    //char *szMessage = a.c_str();
//    send(Socket, a.c_str(), strlen(a.c_str()), 0);

//    // Display message from server
//    char buffer[1000];
//    memset(buffer, 0, 999);
//    int inDataLength = recv(Socket, buffer, 1000, 0);
//    std::cout << buffer << std::endl;
		
//    int nError = WSAGetLastError();
//    if (nError != WSAEWOULDBLOCK&&nError != 0)
//    {
//        std::cout << "Winsock error code: " << nError << "\r\n";
//        std::cout << "Server disconnected!\r\n";
//        // Shutdown our socket
//        shutdown(Socket, SD_SEND);
//        // Close our socket entirely
//        closesocket(Socket);
//    }

//    //int count = 1;
//    //// Main loop
//    //for (;; count++)
//    //{
//    //	std::cout << "This is loop " << count << std::endl;
//    //	// Display message from server
//    //	char buffer[1000];
//    //	memset(buffer, 0, 999);
//    //	int inDataLength = recv(Socket, buffer, 1000, 0);
//    //	std::cout << buffer << std::endl;

//    //	std::string a = "This is client message loop<EOF>";
//    //	//char *szMessage = a.c_str();
//    //	send(Socket, a.c_str(), strlen(a.c_str()), 0);
//    //	
//    //	int nError = WSAGetLastError();
//    //	if (nError != WSAEWOULDBLOCK&&nError != 0)
//    //	{
//    //		std::cout << "Winsock error code: " << nError << "\r\n";
//    //		std::cout << "Server disconnected!\r\n";
//    //		// Shutdown our socket
//    //		shutdown(Socket, SD_SEND);

//    //		// Close our socket entirely
//    //		closesocket(Socket);

//    //		break;
//    //	}
//    //	Sleep(1000);
//    //}

//    // Shutdown our socket
//    shutdown(Socket, SD_SEND);

//    // Close our socket entirely
//    closesocket(Socket);

//    // Cleanup Winsock
//    WSACleanup();
//    //system("PAUSE");
//    return 0;
//}
}
