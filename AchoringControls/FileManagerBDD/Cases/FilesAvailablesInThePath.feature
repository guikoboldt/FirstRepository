Feature: FilesAvailablesInThePath
	The download path receives all files downloaded by the application
	I want to open this path and see what I have downloaded until now by the application

Scenario: List all availables files
	Given This is the download path: 'C:\Download'
	When I click on the FileManager button
	Then The following files should appear
	| Name      |
	| text1.txt |
	| text2.txt |
	| text3.txt |