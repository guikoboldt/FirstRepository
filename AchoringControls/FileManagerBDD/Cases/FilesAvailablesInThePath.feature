Feature: Files in the path
	The DownloadManager receives all files downloaded by the application
	I want to open this feature and see what I have downloaded until now

@Path_initializer
Scenario: List availables files
	Given The following path 'C:\Download'
	When I open the DownloadManager
	Then files should be displayed

@Path_IsEmpty
Scenario: No Files availables
	Given The following path 'C:\Download'
	When I open the DownloadManager
	Then No files should be displayed