﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (https://www.specflow.org/).
//      SpecFlow Version:3.9.0.0
//      SpecFlow Generator Version:3.9.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace LeadManagementSystem.Test.Features
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.9.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public partial class RegisterLeadStatusUpdateFeature : object, Xunit.IClassFixture<RegisterLeadStatusUpdateFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private static string[] featureTags = ((string[])(null));
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "RegisterLeadStatusUpdate.feature"
#line hidden
        
        public RegisterLeadStatusUpdateFeature(RegisterLeadStatusUpdateFeature.FixtureData fixtureData, LeadManagementSystem_Test_XUnitAssemblyFixture assemblyFixture, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Features", "RegisterLeadStatusUpdate", @"  A user is entering details to update the Lead Status.
  The system should verify the details are present in the database.
  If the details are accurate, the system updates the Lead Status.
  If the details are invalid, appropriate error messages should be displayed.", ProgrammingLanguage.CSharp, featureTags);
            testRunner.OnFeatureStart(featureInfo);
        }
        
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        public void TestInitialize()
        {
        }
        
        public void TestTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Xunit.Abstractions.ITestOutputHelper>(_testOutputHelper);
        }
        
        public void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        void System.IDisposable.Dispose()
        {
            this.TestTearDown();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Update lead status with accurate details")]
        [Xunit.TraitAttribute("FeatureTitle", "RegisterLeadStatusUpdate")]
        [Xunit.TraitAttribute("Description", "Update lead status with accurate details")]
        [Xunit.TraitAttribute("Category", "tag1")]
        public void UpdateLeadStatusWithAccurateDetails()
        {
            string[] tagsOfScenario = new string[] {
                    "tag1"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Update lead status with accurate details", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 11
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
                TechTalk.SpecFlow.Table table32 = new TechTalk.SpecFlow.Table(new string[] {
                            "LeadId",
                            "StatusIdFrom",
                            "SubStatusIdFrom",
                            "StatusIdTo",
                            "SubStatusIdTo",
                            "DALASUserName",
                            "BrandID",
                            "BrandAppID"});
                table32.AddRow(new string[] {
                            "216",
                            "1",
                            "17",
                            "2",
                            "10",
                            "testuser123",
                            "B001",
                            "APP01"});
#line 12
    testRunner.Given("I have a lead status update request :", ((string)(null)), table32, "Given ");
#line hidden
#line 15
    testRunner.When("I send a lead status update request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 16
    testRunner.Then("The response should be successful and contain no errors.", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Update lead status with empty LeadId and exceeding BrandAppID length")]
        [Xunit.TraitAttribute("FeatureTitle", "RegisterLeadStatusUpdate")]
        [Xunit.TraitAttribute("Description", "Update lead status with empty LeadId and exceeding BrandAppID length")]
        public void UpdateLeadStatusWithEmptyLeadIdAndExceedingBrandAppIDLength()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Update lead status with empty LeadId and exceeding BrandAppID length", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 19
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
                TechTalk.SpecFlow.Table table33 = new TechTalk.SpecFlow.Table(new string[] {
                            "LeadId",
                            "StatusIdFrom",
                            "SubStatusIdFrom",
                            "StatusIdTo",
                            "SubStatusIdTo",
                            "DALASUserName",
                            "BrandID",
                            "BrandAppID"});
                table33.AddRow(new string[] {
                            "0",
                            "1",
                            "34",
                            "2",
                            "34",
                            "testuser123",
                            "B001",
                            "APP0123456789"});
#line 20
 testRunner.Given("I have a lead status update request :", ((string)(null)), table33, "Given ");
#line hidden
#line 23
 testRunner.When("I send a lead status update request with invalid request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
                TechTalk.SpecFlow.Table table34 = new TechTalk.SpecFlow.Table(new string[] {
                            "fieldName",
                            "errorMessage"});
                table34.AddRow(new string[] {
                            "LeadId",
                            "The leadid must be greater than 0"});
#line 24
 testRunner.Then("The response should indicate validation errors.", ((string)(null)), table34, "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Invalid StatusIdFrom value")]
        [Xunit.TraitAttribute("FeatureTitle", "RegisterLeadStatusUpdate")]
        [Xunit.TraitAttribute("Description", "Invalid StatusIdFrom value")]
        public void InvalidStatusIdFromValue()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Invalid StatusIdFrom value", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 29
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
                TechTalk.SpecFlow.Table table35 = new TechTalk.SpecFlow.Table(new string[] {
                            "LeadId",
                            "StatusIdFrom",
                            "SubStatusIdFrom",
                            "StatusIdTo",
                            "SubStatusIdTo",
                            "DALASUserName",
                            "BrandID",
                            "BrandAppID"});
                table35.AddRow(new string[] {
                            "217",
                            "100",
                            "45",
                            "29",
                            "50",
                            "testuser123",
                            "B001",
                            "APP01"});
#line 30
    testRunner.Given("I have a lead status update request :", ((string)(null)), table35, "Given ");
#line hidden
#line 33
    testRunner.When("I send a lead status update request with invalid request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
                TechTalk.SpecFlow.Table table36 = new TechTalk.SpecFlow.Table(new string[] {
                            "fieldName",
                            "errorMessage"});
                table36.AddRow(new string[] {
                            "StatusIdFrom",
                            "Invalid status id from value: 100"});
#line 34
    testRunner.Then("The response should indicate validation errors.", ((string)(null)), table36, "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Invalid StatusIdTo value")]
        [Xunit.TraitAttribute("FeatureTitle", "RegisterLeadStatusUpdate")]
        [Xunit.TraitAttribute("Description", "Invalid StatusIdTo value")]
        public void InvalidStatusIdToValue()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Invalid StatusIdTo value", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 38
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
                TechTalk.SpecFlow.Table table37 = new TechTalk.SpecFlow.Table(new string[] {
                            "LeadId",
                            "StatusIdFrom",
                            "SubStatusIdFrom",
                            "StatusIdTo",
                            "SubStatusIdTo",
                            "DALASUserName",
                            "BrandID",
                            "BrandAppID"});
                table37.AddRow(new string[] {
                            "216",
                            "1",
                            "43",
                            "99",
                            "41",
                            "testuser123",
                            "B001",
                            "APP01"});
#line 39
    testRunner.Given("I have a lead status update request :", ((string)(null)), table37, "Given ");
#line hidden
#line 43
    testRunner.When("I send a lead status update request with invalid request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
                TechTalk.SpecFlow.Table table38 = new TechTalk.SpecFlow.Table(new string[] {
                            "fieldName",
                            "errorMessage"});
                table38.AddRow(new string[] {
                            "StatusIdTo",
                            "Invalid status id to value: 99"});
#line 44
    testRunner.Then("The response should indicate validation errors.", ((string)(null)), table38, "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Verify lead status update is saved in the database")]
        [Xunit.TraitAttribute("FeatureTitle", "RegisterLeadStatusUpdate")]
        [Xunit.TraitAttribute("Description", "Verify lead status update is saved in the database")]
        public void VerifyLeadStatusUpdateIsSavedInTheDatabase()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Verify lead status update is saved in the database", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 51
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
                TechTalk.SpecFlow.Table table39 = new TechTalk.SpecFlow.Table(new string[] {
                            "LeadId",
                            "StatusIdFrom",
                            "SubStatusIdFrom",
                            "StatusIdTo",
                            "SubStatusIdTo",
                            "DALASUserName",
                            "BrandID",
                            "BrandAppID"});
                table39.AddRow(new string[] {
                            "216",
                            "1",
                            "7",
                            "2",
                            "12",
                            "testuser123",
                            "B001",
                            "APP01"});
#line 52
 testRunner.Given("I have a lead status update request :", ((string)(null)), table39, "Given ");
#line hidden
#line 55
 testRunner.When("I send a lead status update request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 56
 testRunner.Then("The lead status update should be saved in the database.", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Giving lead status update request with explicit id")]
        [Xunit.TraitAttribute("FeatureTitle", "RegisterLeadStatusUpdate")]
        [Xunit.TraitAttribute("Description", "Giving lead status update request with explicit id")]
        public void GivingLeadStatusUpdateRequestWithExplicitId()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Giving lead status update request with explicit id", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 58
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
                TechTalk.SpecFlow.Table table40 = new TechTalk.SpecFlow.Table(new string[] {
                            "Id",
                            "LeadId",
                            "StatusIdFrom",
                            "SubStatusIdFrom",
                            "StatusIdTo",
                            "SubStatusIdTo",
                            "DALASUserName",
                            "BrandID",
                            "BrandAppID"});
                table40.AddRow(new string[] {
                            "217",
                            "216",
                            "1",
                            "1",
                            "2",
                            "5",
                            "testuser123",
                            "B001",
                            "APP01"});
#line 59
 testRunner.Given("I have a lead status update request :", ((string)(null)), table40, "Given ");
#line hidden
#line 62
 testRunner.When("I send a lead status update request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 63
 testRunner.Then("The lead status update should ignore the explicit id and save the update.", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="LeadId is not present in database to update the status")]
        [Xunit.TraitAttribute("FeatureTitle", "RegisterLeadStatusUpdate")]
        [Xunit.TraitAttribute("Description", "LeadId is not present in database to update the status")]
        public void LeadIdIsNotPresentInDatabaseToUpdateTheStatus()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("LeadId is not present in database to update the status", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 65
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
                TechTalk.SpecFlow.Table table41 = new TechTalk.SpecFlow.Table(new string[] {
                            "LeadId",
                            "StatusIdFrom",
                            "SubStatusIdFrom",
                            "StatusIdTo",
                            "SubStatusIdTo",
                            "DALASUserName",
                            "BrandID",
                            "BrandAppID"});
                table41.AddRow(new string[] {
                            "1212121222",
                            "1",
                            "5",
                            "3",
                            "17",
                            "testuser123",
                            "B001",
                            "APP01"});
#line 66
 testRunner.Given("I have a lead status update request :", ((string)(null)), table41, "Given ");
#line hidden
#line 69
 testRunner.When("I send a lead status update request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 70
 testRunner.Then("The lead status update should show an exception if LeadId is missing.", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Passing empty lead status update request")]
        [Xunit.TraitAttribute("FeatureTitle", "RegisterLeadStatusUpdate")]
        [Xunit.TraitAttribute("Description", "Passing empty lead status update request")]
        public void PassingEmptyLeadStatusUpdateRequest()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Passing empty lead status update request", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 73
 this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 74
 testRunner.Given("I have a empty lead status update request :", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 75
 testRunner.When("I send a lead status update request with invalid request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 76
 testRunner.Then("Exception thrown by leadcontroller for empty leadstatusupdate request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.9.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                RegisterLeadStatusUpdateFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                RegisterLeadStatusUpdateFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
