using FluentAssertions;
using TechTalk.SpecFlow;

namespace LedPanelBDD.Steps
{
    [Binding]
    public class DisplayMessagesInPanelSteps
    {
        protected PanelContext context;
        protected string message;

        public DisplayMessagesInPanelSteps(PanelContext context)
        {
            this.context = context;
        }

        [Given(@"'(.*)' as message")]
        public void GivenAsMessage(string message)
        {
            this.message = message;
        }

        [When(@"I send the message")]
        public void WhenISendTheMessageToPanel()
        {
            context.GetPanel.DisplayMessage(message, null, LedPanel.Entities.LedPanel.MessageType.Normal_1L).Wait();
        }
        
        [Then(@"the message should display")]
        public void ThenTheMessageShouldDisplay()
        {
            var socketMessage = context.GetSocket.GetMessageFromServer().Result;
            socketMessage.Should().Be(message.ToUpper(), because: "This assertion is needed to check if the phrase parameter is the same on the panel side");
        }
    }
}
