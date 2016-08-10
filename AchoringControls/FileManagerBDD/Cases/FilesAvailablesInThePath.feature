Feature: FilesAvailablesInThePath
	The download path receives all files downloaded by the application
	I want to open this path and see what I have downloaded until now by the application

@mytag
Scenario: List all availables files
	Given This is the download path: 'Path'
	And there is '1' or more files availables in 'Path'
	When I click on the FileManager button
	Then all files availables in the 'Path' should appear in the screen
