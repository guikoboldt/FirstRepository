using System;
using TechTalk.SpecFlow;

namespace FileManagerBDD.Steps
{
    [Binding]
    public class ListAllAvailablesFilesSteps
    {
        [Given(@"This is the download path: '(.*)'")]
        public void GivenThisIsTheDownloadPath(string downloadPath)
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"I click on the FileManager button")]
        public void WhenIClickOnTheFileManagerButton()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"there is '(.*)' or more files availables in '(.*)'")]
        public void GivenThereIsOrMoreFilesAvailablesIn(int numberAvailablesFiles, string downloadPath)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"all files availables in the '(.*)' should appear in the screen")]
        public void ThenAllFilesAvailablesInTheShouldAppearInTheScreen(string downloadPath)
        {
            ScenarioContext.Current.Pending();
        }

    }
}
