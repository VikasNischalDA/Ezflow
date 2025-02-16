using FluentAssertions;
using LeadManagementSystem.Contracts.Response;
using LeadManagementSystem.Shared.Contracts.Request;
using LeadManagementSystem.Shared.Infrastructure;
using LeadManagementSystem.Test.Support;
using Microsoft.AspNetCore.Mvc;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Table = TechTalk.SpecFlow.Table;

namespace LeadManagementSystem.Test.Features
{
    [Binding]
    public class RegisterLeadStatusUpdateSteps
    {
        private readonly TestContext testContext;

        public RegisterLeadStatusUpdateSteps(TestContext _testContext)
        {
            testContext = _testContext;
        }

        [Given(@"I have a lead status update request :")]
        public void GivenIHaveLeadStatusUpdateRequestWithTheFollowingDetails(Table table)
        {
            testContext.leadStatusUpdateRequest = table.CreateInstance<LeadStatusUpdateRequest>();
        }
        [Given(@"I have a empty lead status update request :")]
        public void GivenIHaveEmptyLeadStatusUpdateRequestWithTheFollowingDetails()
        {
            testContext.leadStatusUpdateRequest = null;
        }
        [When(@"I send a lead status update request")]
        public async Task WhenISendLeadStatusUpdateRequest()
        {
            if (testContext.leadStatusUpdateRequest == null)
            {
                throw new Exception("The LeadStatusUpdate request cannot be null.");
            }

            testContext.leadUpdateResponse = await testContext.leadController.LeadStatusUpdate(testContext.leadStatusUpdateRequest);
            var okResult = testContext.leadUpdateResponse as OkObjectResult;
            okResult.Should().NotBeNull("The result should be of type OkObjectResult.");
            var leadStatusUpdateBody = okResult.Value as LeadStatusUpdateBody;
            testContext.leadStatusUpdateResponse = leadStatusUpdateBody.LeadStatusUpdateResponse;

        }

        [When(@"I send a lead status update request with invalid request")]
        public async Task WhenISendLeadStatusUpdateRequestWithEmptyRequest()
        {

            testContext.leadUpdateResponse = await testContext.leadController.LeadStatusUpdate(testContext.leadStatusUpdateRequest);
            var okResult = testContext.leadUpdateResponse as OkObjectResult;
            okResult.Should().NotBeNull("The result should be of type OkObjectResult.");
            testContext.LeadStatusUpdateActionResult = okResult.Value as Shared.Infrastructure.ActionResult<LeadStatusUpdateResponse>;
        }

        [Then(@"The response should indicate validation errors.")]

        public void ThenReceiveValidationErrors(Table table)
        {
            var errors = testContext.LeadStatusUpdateActionResult.Errors;
            errors.Should().NotBeNullOrEmpty("Validation errors should be present.");

            foreach (var row in table.Rows)
            {
                ValidateError(errors, row["fieldName"], row["errorMessage"]);
            }
        }

        [Then(@"The lead status update should be saved in the database.")]
        public void ThenVerifyLeadStatusUpdateSavedInDatabase()
        {

            AssertResponseSuccess(testContext.leadStatusUpdateResponse);
        }

        [Then(@"The lead status update should show an exception if LeadId is missing.")]
        public void ThenLeadStatusUpdateShouldShowExceptionIfLeadIdMissing()
        {
            AssertException(
                testContext.leadStatusUpdateResponse,

                $"No Lead with 1212121222 found",
                false
            );
        }

        [Then(@"The response should be successful and contain no errors.")]
        public void ThenResponseShouldBeSuccessfulAndContainNoErrors()
        {
            AssertResponseSuccess(testContext.leadStatusUpdateResponse);
        }

        [Then(@"The lead status update should ignore the explicit id and save the update.")]
        public void ThenLeadStatusUpdateShouldIgnoreExplicitId()
        {
            AssertResponseSuccess(testContext.leadStatusUpdateResponse);
        }


        [Then(@"Exception thrown by leadcontroller for empty leadstatusupdate request")]
        public void ThenExceptionThrownByLeadcontrollerForEmptyLeadStatusUpdaterequest()
        {
            testContext.LeadStatusUpdateActionResult.Errors.Should().NotBeNull("The response should contain errors.");

            var validationError = testContext.LeadStatusUpdateActionResult.Errors.FirstOrDefault();


            validationError.ErrorMessage.Should().Contain("Value cannot be null. (Parameter 'request')",
                "LeadController error message mismatch for empty lead request.");


            validationError.FieldName.Should().Contain("Error",
                "Expected 'Error' as the field name in the validation error.");
        }

        private void AssertResponseSuccess(LeadStatusUpdateResponse response)
        {
            response.Should().NotBeNull("Expected response not to be null.");

            response.WebServiceMessage.Should().NotBeNull("Entity should not be null");
            response.WebServiceMessage.Success.Should().BeTrue();
            response.WebServiceMessage.ErrorMessage.Should().BeNull();
        }

        private void ValidateError(IEnumerable<ValidationError> errors, string fieldName, string errorMessage)
        {
            errors.Should().Contain(e => e.FieldName == fieldName && e.ErrorMessage.Contains(errorMessage),
                $"Validation error for field '{fieldName}' should include message '{errorMessage}'.");
        }

        private void AssertException(LeadStatusUpdateResponse response, string expectedMessage, bool isSuccess)
        {

            response.WebServiceMessage.Should().NotBeNull("Entity should not be null.");
            response.WebServiceMessage.Success.Should().Be(isSuccess, "Success flag mismatch.");
            response.WebServiceMessage.ErrorMessage.Should().Be(expectedMessage, "Error message mismatch.");

        }
    }
}