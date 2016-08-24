@requires_socket
Feature: display messages in panel
	In order to display messages in a Led Panel
	As a person who wants to show some message
	I want to write a short sentence and display it

Scenario: Send a single message
	Given 'Test 1' as message
	When I send the message
	Then the message should display

Scenario: Send a double message
	Given 'Test 1' and 'Test 2' as messages
	When I send these messages
	Then these message should display
