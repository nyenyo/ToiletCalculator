Feature: MBIE Toilet Calculator
Specflow training for MBIE Toilet Calculator

@mytag
Scenario: Calculate toilets for a hospital
	Given user navigates to the toilet calculator
	And user starts form with number of people unknown
	And user inputs data for building type <building_type>
	And user inputs data with the metrics of <dining_area>, <interview_area>, <kitchen_area>, <laundry_area>, <lobbies_area>, <offices_area>, <facilities_area>, <reception_area>, <subordinates_area>, <number_of_beds>
	When user submits the form
	And user prints the results
	Then user is able to save the results to PDF

	Examples: 
	| building_type | dining_area | interview_area | kitchen_area | laundry_area | lobbies_area | offices_area | facilities_area | reception_area | subordinates_area | number_of_beds |
	| Hospital      | 20          | 15             | 22           | 23           | 55           | 42           | 34              | 27             | 30                | 100            |

@mytag
Scenario: Calculate toilets for a museum
	Given user navigates to the toilet calculator
	And user starts form with known number of people
	And user inputs data for building type <building_type>
	And user inputs data with metrics of <num_of_people>
	When user submits the form
	And user prints the results
	Then user is able to save the results to PDF

	Examples: 
	| building_type | num_of_people |
	| Museum        | 100           |