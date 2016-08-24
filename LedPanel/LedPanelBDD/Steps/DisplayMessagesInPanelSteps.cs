using FluentAssertions;
using TechTalk.SpecFlow;

namespace LedPanelBDD.Steps
{
    [Binding]
    public class DisplayMessagesInPanelSteps
    {
        protected PanelContext context;
        protected string message1;
        protected string message2;

        public DisplayMessagesInPanelSteps(PanelContext context)
        {
            this.context = context;
        }

        [Given(@"'(.*)' as message")]
        public void GivenAsMessage(string message)
        {
            this.message1 = message;
        }

        [When(@"I send the message")]
        public void WhenISendTheMessageToPanel()
        {
            context.Panel.DisplayMessage(message1, null, LedPanel.Entities.LedPanel.MessageType.Normal_1L).Wait();
        }
        
        [Then(@"the message should display")]
        public void ThenTheMessageShouldDisplay()
        {
            var socketMessage = context.Socket.GetMessageFromServer();
            socketMessage.Should().Be(message1.ToUpper(), because: "This assertion is needed to check if the phrase parameter is the same on the panel's side");
        }

        [Given(@"'(.*)' and '(.*)' as messages")]
        public void GivenAndAsMessages(string message1, string message2)
        {
            this.message1 = message1;
            this.message2 = message2;
        }

        [When(@"I send these messages")]
        public void WhenISendTheseMessages()
        {
            context.Panel.DisplayMessage(message1, message2, LedPanel.Entities.LedPanel.MessageType.Normal_2L).Wait();
        }

        [Then(@"these message should display")]
        public void ThenTheseMessageShouldDisplay()
        {
            var socketMessage = context.Socket.GetMessageFromServer();
            var messages = message1.ToUpper() + " \r\n " + message2.ToUpper();
            socketMessage.Should().Be(messages, "This assertion is needed to check if all messages were display by the panel");
        }

    }
}
