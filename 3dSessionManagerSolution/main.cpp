#include <WinSock2.h>//dont move this for windows.h mess with winsock
#include <stdlib.h> //or this for GLUT mess with exit
#include <Windows.h>
#include <HNWorld.h>
#include <conio.h>

#ifdef _WIN32
#include "HNWindowsObjectFactory.h"
#include "HNWindowsNetworkObjectFactory.h"
#endif
#include "HNAdithScene.h"
#include "HNKinectV1.h"
#include "HNKinectV2.h"
#include "HNKinectDepthFilter.h"
#include "HNDeviceReceiver.h"
#include "HNConnectionManager.h"

#pragma comment(lib,"ws2_32.lib")

const char* SESSION_MANAGEMENT_SERVER = "localhost";
int SESSION_MANAGEMENT_SERVER_PORT = 11000;
const char* HNINSTANCE_ID = "192.168.0.3";

int callSocket(string instanceID,string messageType,string pushMessage);

int main(int argc, char* argv[]) {
	glutInit(&argc, argv);
#ifdef _WIN32
	HNWindowsObjectFactory::setAsDefaultFactory();
	HNWindowsNetworkObjectFactory::setAsDefaultFactory();
#endif
	//HNWorld::getInstance()->setBackcolor(HNPointXYZW(0,0,0,0));
	//HNAdithScene *scene = new HNAdithScene();
	//HNWorld::getInstance()->addScene(scene);
	//HNWorld::getInstance()->start();
	cout << "Running HUNA Instance :" << HNINSTANCE_ID<<endl;
	int count = 1;
	while (1)
	{
		cout << "Sending Message to Server ..." << endl;
		//string msg = "This is message no " + to_string(count) + " from HUNA Instance " + HNINSTANCE_ID;
		string msg = "File Stored in  <a href=\\'/Content/Temp/Instance/SkeletonData1.js\\' target=\\'_blank\\'>ftp:\\\\192.168.0.3\\\\FileStore\\\\Instance1\\\\SkeletonData" + to_string(count) + ".data</a>";
		cout << msg<<endl;
		callSocket(HNINSTANCE_ID,"FILE_LOCATION",msg);
		count++;
		system("PAUSE");
	}
	_getch();
}

int callSocket(string instanceID, string messageType, string pushMessage)
{
	// Initialise Winsock
	WSADATA WsaDat;
	if (WSAStartup(MAKEWORD(2, 2), &WsaDat) != 0)
	{
		std::cout << "Winsock error - Winsock initialization failed\r\n";
		WSACleanup();
		//system("PAUSE");
		return 0;
	}

	// Create our socket
	SOCKET Socket = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
	if (Socket == INVALID_SOCKET)
	{
		std::cout << "Winsock error - Socket creation Failed!\r\n";
		WSACleanup();
		//system("PAUSE");
		return 0;
	}

	// Resolve IP address for hostname
	struct hostent *host;
	if ((host = gethostbyname(SESSION_MANAGEMENT_SERVER)) == NULL)
	{
		std::cout << "Failed to resolve hostname.\r\n";
		WSACleanup();
		//system("PAUSE");
		return 0;
	}

	// Setup our socket address structure
	SOCKADDR_IN SockAddr;
	SockAddr.sin_port = htons(SESSION_MANAGEMENT_SERVER_PORT);
	SockAddr.sin_family = AF_INET;
	SockAddr.sin_addr.s_addr = *((unsigned long*)host->h_addr);

	// Attempt to connect to server
	if (connect(Socket, (SOCKADDR*)(&SockAddr), sizeof(SockAddr)) != 0)
	{
		std::cout << "Failed to establish connection with server\r\n";
		WSACleanup();
		//system("PAUSE");
		return 0;
	}	

	// If iMode!=0, non-blocking mode is enabled.
	u_long iMode = 1;
	ioctlsocket(Socket, FIONBIO, &iMode);
	string sendMessage = "{'id':'" + instanceID + "','dataType':'" + messageType + "','data':'" + pushMessage + "'}";
	std::string a = sendMessage + "<EOF>";	
	send(Socket, a.c_str(), strlen(a.c_str()), 0);

	// Display message from server
	char buffer[1000];
	memset(buffer, 0, 999);
	int inDataLength = recv(Socket, buffer, 1000, 0);
	std::cout << buffer << std::endl;
		
	int nError = WSAGetLastError();
	if (nError != WSAEWOULDBLOCK&&nError != 0)
	{
		std::cout << "Winsock error code: " << nError << "\r\n";
		std::cout << "Server disconnected!\r\n";
		// Shutdown our socket
		shutdown(Socket, SD_SEND);
		// Close our socket entirely
		closesocket(Socket);
	}

	// Shutdown our socket
	shutdown(Socket, SD_SEND);

	// Close our socket entirely
	closesocket(Socket);

	// Cleanup Winsock
	WSACleanup();	
	return 1;
}

