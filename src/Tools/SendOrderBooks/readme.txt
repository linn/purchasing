A Simple command line program to manually resend emails with order book spreadsheets as set up here: https://app.linn.co.uk/purch/planning/plautoem.aspx

args[0] - test - bool - Whether to send to the real supplier or a specified test address
args[1] - suppliers - string - A comma seperated list of all the supplierIds you want to include, or the string 'all' to send to all.
args[2] - test address - string - optionally specify an address to send the emails to if args[0] is true

Example: Send to all suppliers for real

dotnet run false all

Example send to a test address for selected suppliers 

dotnet run true 12345,456778,333456 youremail@domain.com

you will also need a config.env file in the same directory as the executable - see config-example.env
