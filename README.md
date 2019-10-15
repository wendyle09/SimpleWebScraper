# SimpleWebScraper

## Acceptance Criteria
The user would like to scrape links and their associated descriptions from Craigslist's based on the category and city specified by the user.

Input:
* City
* Category abbreviation

Output:
* Item description
* Item URL

### Assumptions
1. User knows the abbreviation for the category they would like to scrape, such as "ata" for antiques.
2. User does not misspell city name.
3. Craigslist listing pages follow this format: http://CITY.craigslist.org/search/CATEGORY

## Algorithm
```
City = input from user
Category = input from user

if City and Category are valid then download specified Craigslist listing contents
	grab a specified HTML element based on regular expression
		loop through each matched HTML element
		if no specified HTML parts
				add matched HTML elements to scraped items listing
			otherwise
				loop through each HTML specified part
				grab specified HTML part from matched HTML element based on regular expression
					if match found
						add matched HTML part to scraped items list
otherwise
	show warning

if scraped items list is not empty
	display scraped HTML elements/parts
```
