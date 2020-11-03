Feature: CreateBooking
	In order to rest pecefully on my holidays
	As a hotel customer
	I want to know if booking my room was successful

@mytag
Scenario: Book a room for period of time
	Given I have entered a start date <startDate>
	And I have entered an end date <endDate> 
	When I book a room
	Then I want to know if there are available rooms <result>

	Examples:
    | startDate  |   endDate  | result  |
    |  11/9/2020 |  11/9/2020 |  true   |  
	| 11/16/2020 | 11/16/2020 |  true   |
	|  11/9/2020 | 11/16/2020 | false   |
	|  11/9/2020 | 11/10/2020 | false   |
	|  11/9/2020 | 11/15/2020 | false   |
	| 11/10/2020 | 11/16/2020 | false   |
	| 11/15/2020 | 11/16/2020 | false   |
	| 11/10/2020 | 11/10/2020 | false   |
	| 11/10/2020 | 11/15/2020 | false   |
	| 11/15/2020 | 11/15/2020 | false   |

 # Booking { Id = 2, CustomerId = 2, EndDate = new DateTime(2020, 11, 10), StartDate = new DateTime(2020, 11, 15), IsActive = true, RoomId = 2  },