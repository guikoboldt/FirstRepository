Feature: FilesAvailablesInThePath
	The download path receives all files downloaded by the application
	I want to open this path and see what I have downloaded until now by the application

	@Path_initializer
Scenario: List all availables files
	Given This is the download path: 'C:\Download'
	When I click on the DownloadManager button
	Then files should be found

Scenario: No Files availables
	Given This is the download path: 'C:\Download'
	When I click on the DownloadManager button
	Then I should see a message : No files found