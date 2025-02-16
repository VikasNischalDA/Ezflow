
Feature: Register Lead
    In order to manage leads
    As a user
    I want to be able to register a new lead successfully


Scenario: Attempt to register a new lead with accurate details.
	Given I have a lead request with the following details:
		| IDNumber      | FirstName | Surname | CellPhone     | AlternateNumber | Email             | SupplierEmail     | UMID     | SupplierSource | SMSresponse | Option  | WorkNumber    | HomeNumber    | PermissionToPromote | AllowsCreditCheck | GrossIncome |
		| 1234567890123 | TestJohn  | TestDoe | 0822345673 | 0822345673   | john.doe@test.com | supplier@test.com | 25498756 | Source1        | Yes         | Option1 | 0822345673 | 0822345673 | yes                 | true              | 50000       |
	When I attempt to register the lead request
	Then The response should indicate success

Scenario: Attempt to register a lead with  invalid telephone number fields, IDNumber, and other  fields.
	Given I have a lead request with the following details:
		| IDNumber        | FirstName | Surname | CellPhone | AlternateNumber | Email             | SupplierEmail     | UMID  | SupplierSource | SMSresponse | Option  | WorkNumber | HomeNumber | PermissionToPromote | AllowsCreditCheck | GrossIncome |
		| 123456789012342 | TestJohn  | TestDoe | 123455    | 0822345673   | john.doe@test.com | supplier@test.com | 45678 | Source1        | Yes         | Option1 | 111111     | 123456789  | yes                 | true              |             |
	When I attempt to register the lead request with invalid request
	Then The response should show validation errors for the following fields
		| fieldName  | errorMessage                                       |
		| IDNumber   | The IDNumber must be exactly 13 characters long.   |
		| CellPhone  | Cell phone number must begin with '07' or '08'. |
		| WorkNumber | Work number provided is in invalid format.     |
		| HomeNumber |Home number provided is in invalid format.       |
		| UMID       | UMID must be of 8 digits.                          |
        
Scenario: Attempt to register a lead with an invalid Gross Income value.
	Given I have a lead request with the following details:
		| IDNumber      | FirstName | Surname | CellPhone     | AlternateNumber | Email             | SupplierEmail     | UMID     | SupplierSource | SMSresponse | Option  | WorkNumber    | HomeNumber    | PermissionToPromote | AllowsCreditCheck | GrossIncome |
		| 1Abs123456789 | TestJohn@  | TestDoe@ | 0822345673 | 0822345673   | john.doe@test.com | supplier@test.com | 123U5678 | Source1        | Yes         | Option1 | 0822345673 | 0822345673 | yes                 | true              | 1000000     |
	When I attempt to register the lead request with invalid request
	Then The response should show validation errors for the following fields
		| fieldName   | errorMessage                                            |
		| GrossIncome | Gross Income must be between 0 and 999999.              |
		| IDNumber    | The IDNumber must contain only numbers.                 |
		| FirstName   | FirstName cannot contain numbers or special characters. |
		| Surname     | SurName cannot contain numbers or special characters.   |
		| UMID        |UMID must contain only numbers.                          |
Scenario: Attempt to register a lead with identical Email and SupplierEmail fields and empty mandatory fields.
	Given I have a lead request with the following details:
		| IDNumber | FirstName | Surname | CellPhone | AlternateNumber | Email             | SupplierEmail     | UMID | SupplierSource | SMSresponse | Option  | WorkNumber    | HomeNumber    | PermissionToPromote | AllowsCreditCheck | GrossIncome |
		|          |           |         |           | 0822345673   | john.doe@test.com | john.doe@test.com |      | Source1        | Yes         | Option1 | 0822345673 | 0822345673 | yes                 |                   | 123         |
	When I attempt to register the lead request with invalid request
	Then The response should show validation errors for the following fields
		| fieldName         | errorMessage                       |
		| IDNumber          | The IDNumber must not be empty.    |
		| UMID              | UMID cannot be empty.              |
		| FirstName         | FirstName cannot be empty.         |
		| Surname           | SurName cannot be empty.           |
		| CellPhone         | Cell phone number provided is in invalid format. |
		

Scenario: Attempt to register a lead with a missing UMID.
	Given I have a lead request with the following details:
		| IDNumber      | FirstName | Surname | CellPhone     | AlternateNumber | Email             | SupplierEmail     | UMID     | SupplierSource | SMSresponse | Option  | WorkNumber    | HomeNumber    | PermissionToPromote | AllowsCreditCheck | GrossIncome |
		| 1234567890123 | TestJohn  | TestDoe | 0822345673 | 0822345673   | test-not-an-email | test-not-an-email | 12345678 | Source1        | Yes         | Option1 | 0822345673 | 0822345673 | no                  | true              | 123         |
	When I attempt to register the lead request with invalid request
	Then The response should show validation errors for the following fields
		| fieldName     | errorMessage                                   |
		| Email         | Please provide a valid email address.          |
		| SupplierEmail | Please provide a valid Supplier Email address. |

Scenario: Attempt to register a lead with an invalid FirstName, Surname.
	Given I have a lead request with the following details:
		| IDNumber      | FirstName                                               | Surname                                                 | CellPhone     | AlternateNumber | Email             | SupplierEmail     | UMID     | SupplierSource | SMSresponse               | Option  | WorkNumber    | HomeNumber    | PermissionToPromote | AllowsCreditCheck | GrossIncome |
		| 1234567890123 | Simulateanexceptiontotriggerthevalidationblockinthecode | Simulateanexceptiontotriggerthevalidationblockinthecode | 082234573 | 0522345675   | john.doe@test.com | supplier@test.com | 12345678 | Source1        | ResponseExceedingMaxLimit | Option1 | +919878634821 | +911234567891 | yes                 | true              | 123         |
	When I attempt to register the lead request with invalid request
	Then The response should show validation errors for the following fields
		| fieldName | errorMessage                           |
		| FirstName | FirstName cannot exceed 50 characters. |
		| Surname   | SurName cannot exceed 50 characters.   |
		|CellPhone|Cell phone number must be 10 digits long.|
 
Scenario: Attempt to register a lead with a FirstName, Surname with numbers.
	Given I have a lead request with the following details:
		| IDNumber      | FirstName | Surname | CellPhone     | AlternateNumber | Email             | SupplierEmail     | UMID     | SupplierSource | SMSresponse               | Option  | WorkNumber    | HomeNumber    | PermissionToPromote | AllowsCreditCheck | GrossIncome |
		| 1234567890123 | 12334     | 1234    | 0822345673 | 0822345673   | john.doe@test.com | supplier@test.com | 12345678 | Source1        | ResponseExceedingMaxLimit | Option1 | 0822345673 | 0822345673 | yes                 | true              | 123         |
	When I attempt to register the lead request with invalid request
	Then The response should show validation errors for the following fields
		| fieldName | errorMessage                                            |
		| FirstName | FirstName cannot contain numbers or special characters. |
		| Surname   | SurName cannot contain numbers or special characters.   |

Scenario: Attempt to register a lead with an invalid PermissionToPromote and AllowCreditCheck value.
	Given I have a lead request with the following details:
		| IDNumber      | FirstName | Surname | CellPhone     | AlternateNumber | Email             | SupplierEmail     | UMID     | SupplierSource | SMSresponse | Option  | WorkNumber    | HomeNumber    | PermissionToPromote | AllowsCreditCheck | GrossIncome |
		| 1234567890123 | TestJohn  | TestDoe | 0822345673 | 0522345675   | john.doe@test.com | supplier@test.com | 12345678 | Source1        | Yes         | Option1 | 0822345673 | 0822345673 | Maybe               |                   | 123         |
	When I attempt to register the lead request with invalid request
	Then The response should show validation errors for the following fields
		| fieldName           | errorMessage                                 |
		| PermissionToPromote | Permission to Promote must be 'yes' or 'no'. |
		

Scenario: Attempt to register a lead and verify if it's saved in the database.
	Given I have a lead request with the following details:
		| IDNumber      | FirstName | Surname | CellPhone     | AlternateNumber | Email             | SupplierEmail     | UMID     | SupplierSource | SMSresponse | Option  | WorkNumber    | HomeNumber    | PermissionToPromote | AllowsCreditCheck | GrossIncome |
		| 1234567890234 | TestJohn  | TestDoe | 0822345673 | 0522345675   | john.doe@test.com | supplier@test.com | 25498756 | Source1        | Yes         | Option1 | 0822345673 | 0822345673 | yes                 | true              | 50000       |
	When I attempt to register the lead request
	Then Save the lead in the database and verify it's saved.

Scenario: Attempt to register a lead with an explicit ID value that is auto-generated.
	Given I have a lead request with the following details:
		| Id | IDNumber      | FirstName | Surname | CellPhone     | AlternateNumber | Email             | SupplierEmail     | UMID     | SupplierSource | SMSresponse | Option  | WorkNumber    | HomeNumber    | PermissionToPromote | AllowsCreditCheck | GrossIncome |
		| 1  | 1234567890234 | TestJohn  | TestDoe | 0812345678 | 0522345675   | john.doe@test.com | supplier@test.com | 25498756 | Source1        | Yes         | Option1 | 0822345673 | 0621234567 | yes                 | true              | 1200        |
	When I attempt to register the lead request
	Then Exception for auto-generated id should be shown.

Scenario: Attempt to register a lead and receive an exception error stating "UMID not found."
	Given I have a lead request with the following details:
		| IDNumber      | FirstName | Surname | CellPhone     | AlternateNumber | Email             | SupplierEmail     | UMID     | SupplierSource | SMSresponse | Option  | WorkNumber    | HomeNumber    | PermissionToPromote | AllowsCreditCheck | GrossIncome |
		| 1234567890222 | TestJohn  | TestDoe | 0822345673 | 0522345675   | john.doe@test.com | supplier@test.com | 12345678 | Source1        | Yes         | Option1 | 0822345673 | 0822345673 | yes                 | true              | 50000       |
   
	When I attempt to register the lead request
	Then The response should contain an exception error indicating that the UMID was not found.

Scenario: Attempt to register a lead and receive an exception indicating that the BrandId corresponding to the UMID was not found.
	Given I have a lead request with the following details:
		| IDNumber      | FirstName | Surname | CellPhone     | AlternateNumber | Email             | SupplierEmail     | UMID     | SupplierSource | SMSresponse | Option  | WorkNumber    | HomeNumber    | PermissionToPromote | AllowsCreditCheck | GrossIncome |
		| 1234567894567 | TestJohn  | TestDoe | 0822345673 | 0522345675   | john.doe@test.com | supplier@test.com | 79907890 | Source1        | Yes         | Option1 | 0822345673 | 0822345673 | yes                 | true              | 50000       |
   
	When I attempt to register the lead request
	Then Response should indicate missing BrandId for UMID

Scenario Outline: Attempt to register a lead and handle various responses from the LES provider.
	Given I have a lead request with the following details:
		| IDNumber      | FirstName   | Surname   | CellPhone   | AlternateNumber   | Email   | SupplierEmail   | UMID   | SupplierSource   | SMSresponse   | Option   | WorkNumber   | HomeNumber   | PermissionToPromote   | AllowsCreditCheck   | GrossIncome   |
		|<IdNumber> | <FirstName> | <Surname> | <CellPhone> | <AlternateNumber> | <Email> | <SupplierEmail> | <UMID> | <SupplierSource> | <SMSresponse> | <Option> | <WorkNumber> | <HomeNumber> | <PermissionToPromote> | <AllowsCreditCheck> | <GrossIncome> |
	When CreateLeadHandler receives <Decision> from LES provider
	Then The response should indicate success or failure based on the decision <Decision>.

Examples:
	|IdNumber| FirstName | Surname | CellPhone     | AlternateNumber | Email             | SupplierEmail     | UMID     | SupplierSource | SMSresponse | Option  | WorkNumber    | HomeNumber    | PermissionToPromote | AllowsCreditCheck | GrossIncome | Decision |
	|1212131314141| TestJohn  | TestDoe | 0722345675 | 0722345675   | john.doe@test.com | supplier@test.com | 25498756 | Source1        | Yes         | Option1 | 0822345673 | 0822345673 | yes                 | true              | 50000       | Declined |
	|8888889999999| TestJai   | TestDoe | 0722345675 | 0522345675   | jai.doe@test.com  | supplier@test.com | 25498756 | Source1        | Yes         | Option1 | 0822345673 | 0822345673 | yes                 | true              | 50000       | MayBe    |
	|0000122212121| TestJas   | TestDoe | 0722345675 | 0522345675   | jas.doe@test.com  | supplier@test.com | 25498756 | Source1        | Yes         | Option1 | 0822345673 | 0821234567 | yes                 | true              | 50000       | Approved |

Scenario: An exception for "no host known" should be thrown by the DBS provider.
	Given I have a lead request with the following details:
		| IDNumber      | FirstName | Surname     | CellPhone     | AlternateNumber | Email              | SupplierEmail      | UMID     | SupplierSource | SMSresponse | Option  | WorkNumber    | HomeNumber    | PermissionToPromote | AllowsCreditCheck | GrossIncome |
		| 1221221221221 | TestJohn  | Testinvalid | 0822345673 | 0522345675   | john.doe@gmail.com | supplier@gmail.com | 25498756 | Source1        | Yes         | Option1 | 0822345673 | 0822345673 | yes                 | true              | 50000       |
   
	When I attempt to register the lead request
	Then DbsProvider request result into No Host Error

Scenario: An exception should be thrown by LES provider no host known
	Given I have a lead request with the following details:
		| IDNumber      | FirstName | Surname       | CellPhone     | AlternateNumber | Email              | SupplierEmail      | UMID     | SupplierSource | SMSresponse | Option  | WorkNumber    | HomeNumber    | PermissionToPromote | AllowsCreditCheck | GrossIncome |
		| 4545454512121 | TestJohn  | TestDoenohost | 0822345673 | 0522345675   | john.doe@gmail.com | supplier@gmail.com | 25498756 | Source1        | Yes         | Option1 | 0822345673 | 0822345673 | yes                 | true              | 50000       |
   
	When I attempt to register the lead request
	Then LESProvider request result into No Host Error

Scenario: An exception should be thrown by DALAS provider no host known
	Given I have a lead request with the following details:
		| IDNumber      | FirstName | Surname        | CellPhone     | AlternateNumber | Email              | SupplierEmail      | UMID     | SupplierSource | SMSresponse | Option  | WorkNumber    | HomeNumber    | PermissionToPromote | AllowsCreditCheck | GrossIncome |
		| 1234512345123 | TestJohn  | TestDoeINVALID | 0822345673 | 0522345675   | john.doe@gmail.com | supplier@gmail.com | 25498756 | Source1        | Yes         | Option1 | 0822345673 | 0822345673 | yes                 | true              | 50000       |
   
	When I attempt to register the lead request
	Then DalasProvider request result into No Host Error

Scenario: An exception should be thrown by DALAS provider no host known for customer status update
	Given I have a lead request with the following details:
		| IDNumber      | FirstName | Surname | CellPhone     | AlternateNumber | Email              | SupplierEmail      | UMID     | SupplierSource | SMSresponse | Option  | WorkNumber    | HomeNumber    | PermissionToPromote | AllowsCreditCheck | GrossIncome |
		| 1111111111111 | TestJohn  | TestDoe | 0822345673 | 0522345675   | john.doe@gmail.com | supplier@gmail.com | 25498756 | Source1        | Yes         | Option1 | 0822345673 | 0822345673 | yes                 | true              | 50000       |
   
	When I attempt to register the lead request
	Then DalasProvider for customer status request result into No Host Error


Scenario: Attempt to register a lead and receive: Empty response from dalas provider
	Given I have a lead request with the following details:
		| IDNumber      | FirstName | Surname          | CellPhone     | AlternateNumber | Email              | SupplierEmail      | UMID     | SupplierSource | SMSresponse | Option  | WorkNumber    | HomeNumber    | PermissionToPromote | AllowsCreditCheck | GrossIncome |
		| 2221212111333 | TestJohn  | TestDoeempty | 0822345673 | 0522345675   | john.doe@gmail.com | supplier@gmail.com | 25498756 | Source1        | Yes         | Option1 | 0822345673 | 0822345673 | yes                 | true              | 50000       |
   
	When I attempt to register the lead request
	Then DalasProvider request result into empty response

Scenario: Attempt to register a lead and receive: "Application unsuccessful; reapply in 2 months."
	Given I have a lead request with the following details:
		| IDNumber      | FirstName | Surname          | CellPhone     | AlternateNumber | Email              | SupplierEmail      | UMID     | SupplierSource | SMSresponse | Option  | WorkNumber    | HomeNumber    | PermissionToPromote | AllowsCreditCheck | GrossIncome |
		| 1234512345123 | TestJohn  | TestDoetwomonths | 0822345673 | 0522345675   | john.doe@gmail.com | supplier@gmail.com | 25498756 | Source1        | Yes         | Option1 | 0822345673 | 0822345673 | yes                 | true              | 50000       |
   
	When I attempt to register the lead request
	Then Dalas provider's message indicates application unsuccessful

Scenario: Attempt to register a lead Dalas provider give message Cancelled: Message To be confirmed
	Given I have a lead request with the following details:
		| IDNumber      | FirstName | Surname       | CellPhone     | AlternateNumber | Email              | SupplierEmail      | UMID     | SupplierSource | SMSresponse | Option  | WorkNumber    | HomeNumber    | PermissionToPromote | AllowsCreditCheck | GrossIncome |
		| 1234512345123 | TestJohn  | TestDoecancel | 0822345673 | 0522345675   | john.doe@gmail.com | supplier@gmail.com | 25498756 | Source1        | Yes         | Option1 | 0822345673 | 0822345673 | yes                 | true              | 50000       |
   
	When I attempt to register the lead request
	Then Dalas provider gives message Cancelled: Message To be confirmed

Scenario: Attempt to register a lead Dalas provider give message Cancelled: Consent Declined
	Given I have a lead request with the following details:
		| IDNumber      | FirstName | Surname        | CellPhone     | AlternateNumber | Email              | SupplierEmail      | UMID     | SupplierSource | SMSresponse | Option  | WorkNumber    | HomeNumber    | PermissionToPromote | AllowsCreditCheck | GrossIncome |
		| 1234512345123 | TestJohn  | TestDoeconsent | 0822345673 | 0522345675   | john.doe@gmail.com | supplier@gmail.com | 25498756 | Source1        | Yes         | Option1 | 0822345673 | 0822345673 | yes                 | true              | 50000       |
   
	When I attempt to register the lead request
	Then Dalas provider gives message Cancelled: Consent Declined
	
Scenario: An exception should be thrown by DALAS provider application status is null
	Given I have a lead request with the following details:
		| IDNumber      | FirstName | Surname        | CellPhone     | AlternateNumber | Email              | SupplierEmail      | UMID     | SupplierSource | SMSresponse | Option  | WorkNumber    | HomeNumber    | PermissionToPromote | AllowsCreditCheck | GrossIncome |
		| 1212122121212 | TestJohn  | TestDoe | 0822345673 | 0522345675   | john.doe@gmail.com | supplier@gmail.com | 25498756 | Source1        | Yes         | Option1 | 0822345673 | 0822345673 | yes                 | true              | 50000       |
   
	When I attempt to register the lead request
	Then GetCustomerStatus request result exception application status is null

	Scenario: An exception should be thrown by DALAS provider response is null
	Given I have a lead request with the following details:
		| IDNumber      | FirstName | Surname        | CellPhone     | AlternateNumber | Email              | SupplierEmail      | UMID     | SupplierSource | SMSresponse | Option  | WorkNumber    | HomeNumber    | PermissionToPromote | AllowsCreditCheck | GrossIncome |
		| 1122112111211 | TestJohn  | TestDoeempty | 0822345673 | 0522345675   | john.doe@gmail.com | supplier@gmail.com | 25498756 | Source1        | Yes         | Option1 | 0822345673 | 0822345673 | yes                 | true              | 50000       |
   
	When I attempt to register the lead request
	Then Dalas provider request result exception dalas response is null

Scenario: An exception should be thrown by GetCustomerStatus response is empty
Given I have a lead request with the following details:
		| IDNumber      | FirstName | Surname        | CellPhone     | AlternateNumber | Email              | SupplierEmail      | UMID     | SupplierSource | SMSresponse | Option  | WorkNumber    | HomeNumber    | PermissionToPromote | AllowsCreditCheck | GrossIncome |
		| 0000000000000 | TestJohn  | TestDoe | 0822345673 | 0522345675   | john.doe@gmail.com | supplier@gmail.com | 25498756 | Source1        | Yes         | Option1 | 0822345673 | 0822345673 | yes                 | true              | 50000       |
   
	When I attempt to register the lead request 
	Then GetCustomerStatus request result exception response is empty




	Scenario: An exception should be thrown by Leadcontroller
	Given I have a lead request with the empty details:
	When I attempt to register the lead request with empty request
    Then Exception thrown by leadcontroller for empty leadrequest