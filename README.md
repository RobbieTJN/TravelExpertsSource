# TravelExpertsSource
Source code for my Travel Experts web app, with sensitive information removed.

Link to the published website: https://travelexpertswebapplication.azurewebsites.net

If you want to use this yourself, you need to edit the following files:

#### TravelExpertsWebApplication\web.config:

Line 21: Custom Errors needs to be turned on, once you're ready to publish. If this is left at "off", users will see a stack trace instead of a nicer error page

Line 63: Connection string needs to be configured to whichever database you're using

#### TravelExpertsWebApplication\Models\TravelExpertsDB.cs:

Line 14: Connection string needs to be configured to whichever database you're using

I'm using an Azure-hosted database, so if you do the same, you can just replace:

*data source={DATABASE};initial catalog={DATABASE_NAME};user id={USERNAME};password={PASSWORD};*

with the relevant information. Do not include the {curly braces}.

Additionally, as I'm using Entity Framework to connect to the database, you should rebuild the Entity Framework model from your database connection. Make sure you save the existing model classes in a temporary folder before doing this, or you'll lose the data annotations I've added. They can be added in once the new model is generated.

Alternatively, you could not use EF and create database-level classes instead, but that would involve writing a lot of code, both for the database connection and for validation.

### Things to do/Known bugs:

* Password hashing - salt was not working, so is disabled for now
* Verify credit card expiry date if user had previously entered their information. Currently, a user can register a card with a valid expiry date, but if that date passes, the program won't check for that
* Allow entering and changing of credit card information on dashboard
* Better CSS (I was mostly focused on backend programming)

### Future features I'd like to implement:

* Improve credit card entry (CVV, better date selection; requires changes to database)
* Email confirmation
* Password reset
* Third-party authentication (Facebook, Twitter, etc.)
