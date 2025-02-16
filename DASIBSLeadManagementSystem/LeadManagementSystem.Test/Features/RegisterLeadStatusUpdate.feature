Feature: RegisterLeadStatusUpdate

  A user is entering details to update the Lead Status.
  The system should verify the details are present in the database.
  If the details are accurate, the system updates the Lead Status.
  If the details are invalid, appropriate error messages should be displayed.

@tag1


Scenario: Update lead status with accurate details
    Given I have a lead status update request :
        | LeadId | StatusIdFrom | SubStatusIdFrom | StatusIdTo | SubStatusIdTo | DALASUserName | BrandID | BrandAppID |
        | 216      | 1            | 17              | 2          | 10            | testuser123       | B001   | APP01      |
    When I send a lead status update request
    Then The response should be successful and contain no errors.


Scenario: Update lead status with empty LeadId and exceeding BrandAppID length
	Given I have a lead status update request :
		| LeadId | StatusIdFrom | SubStatusIdFrom | StatusIdTo | SubStatusIdTo | DALASUserName | BrandID | BrandAppID |
		| 0      | 1            | 34               | 2          |34            |testuser123| B001   | APP0123456789 |
	When I send a lead status update request with invalid request
	Then The response should indicate validation errors.
		| fieldName | errorMessage                      |
		| LeadId    | The leadid must be greater than 0 |
		

Scenario: Invalid StatusIdFrom value
    Given I have a lead status update request :
      | LeadId | StatusIdFrom | SubStatusIdFrom | StatusIdTo | SubStatusIdTo | DALASUserName | BrandID | BrandAppID |
      | 217      | 100          | 45    | 29         | 50  | testuser123       | B001   | APP01      |
    When I send a lead status update request with invalid request
    Then The response should indicate validation errors.
      | fieldName    | errorMessage                      |
      | StatusIdFrom | Invalid status id from value: 100 |

Scenario: Invalid StatusIdTo value
    Given I have a lead status update request :
      | LeadId | StatusIdFrom | SubStatusIdFrom | StatusIdTo | SubStatusIdTo |DALASUserName | BrandID | BrandAppID |

      | 216      | 1            | 43    | 99         | 41  | testuser123       | B001   | APP01      |
    When I send a lead status update request with invalid request
    Then The response should indicate validation errors.
      | fieldName  | errorMessage                      |
      | StatusIdTo | Invalid status id to value: 99    |




Scenario: Verify lead status update is saved in the database
	Given I have a lead status update request :
		| LeadId | StatusIdFrom | SubStatusIdFrom | StatusIdTo | SubStatusIdTo | DALASUserName | BrandID | BrandAppID |
		| 216    | 1            | 7             | 2          | 12            |  testuser123       | B001   | APP01      |
	When I send a lead status update request
	Then The lead status update should be saved in the database.

Scenario: Giving lead status update request with explicit id
	Given I have a lead status update request :
		| Id | LeadId | StatusIdFrom | SubStatusIdFrom | StatusIdTo | SubStatusIdTo | DALASUserName | BrandID | BrandAppID |
		| 217  | 216    | 1            | 1             | 2          | 5             | testuser123       | B001   | APP01      |
	When I send a lead status update request
	Then The lead status update should ignore the explicit id and save the update.

Scenario: LeadId is not present in database to update the status
	Given I have a lead status update request :
		| LeadId | StatusIdFrom | SubStatusIdFrom | StatusIdTo | SubStatusIdTo | DALASUserName | BrandID | BrandAppID |
		| 1212121222    | 1            | 5               | 3          | 17             | testuser123       | B001   | APP01      |
	When I send a lead status update request
	Then The lead status update should show an exception if LeadId is missing.


	Scenario: Passing empty lead status update request
	Given I have a empty lead status update request :
	When I send a lead status update request with invalid request
	Then Exception thrown by leadcontroller for empty leadstatusupdate request