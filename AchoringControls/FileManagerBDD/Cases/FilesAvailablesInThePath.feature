Feature: FilesAvailablesInThePath
	The download path receives all files downloaded by the application
	I want to open this path and see what I have downloaded until now by the application

	@Path_initializer
Scenario: List all availables files
	Given The following path 'C:\Download'
	When I open the DownloadManager
	Then all giles should be shown
	@Path_IsEmpty
Scenario: No Files availables
	Given The following path 'C:\Download'
	When I open the DownloadManager
	Then I should see a message : No files found