Current User:
    email: vsmith89@health.org
    password: VeronicaMay89!
Sprint 4 Tasks:
    Write comments. ✅
    Test all functionality ✅
    Switch grade with date of birth on form. ✅
    Add option after form for student to save their respones for FERPA reasons (students need the ability to see the information they entered) ✅
    Remove grade and trainer comments from database as they are not necessary. ✅
    Encrypt school name, first name, last name, sport, date of birth, and contact phone number in addition to the password in the database to comply with HIPAA.
        This one is somewhat completed because school name and password are stored successfully encrypted and hashed respectively.
        The reason it's not fully completed is because we search on the athlete form information and the way we had originally planned to encrypt it (every form would have its own key and iv stored in the database), wouldn't work to search on values, so we're a little stuck on how to procede with encrypting that information which is necessary to comply with HIPAA. We'll look more into if there is a package to use for encryption/decryption that will secure the information and not leave it prone to attacks on the data.
    Update UserLogin file to connect to the database. ✅
    Edit BusinessLogic and Database functionality to login with a hashed password and show error for invalid log in. ✅
    Update the Database to use the hashed password (using BCrypt) to create an account. ✅
    Update UserProfile to use the information returned from the database to show the binding information. ✅
    Implement BusinessLogic and Database for UserProfile functionality, including getting the user's information. ✅
    Update the user profile edit button to create entries for school name and password so the user can update their information. ✅
    Create BusinessLogic and Database functionality to update a user's profile information. ✅
    Implement log out functionality from the UserProfile page. ✅
    Implement delete account functionality from the UserProfile page. ✅
    Create BusinessLogic and Database functionality to delete an account. ✅
Sprint 3 Changes:
    Got TrainerHomeScreen connected and functional with the database. 😀😀😀
    Got AthleteInformaiton connected and functional with the database.
    Got InjuryStatistics connected and functional with the database. 😀😀
    Changed navigation for the form tab to connect directly to the InjuryFormReport.
    Fixed design on various screens to be more cohesive and use the same color scheme.
    Got AthletePastForms connected and functional with the database.
    Got AthleteFormInformation connected and functional with the database.
    Got AthleteFormInformationEdit connected and functional with the database. 😀
    Updated AthleteStatuses so trainers can update statuses from the screen.    
Sprint 2 Changes: 
    Added an athlete's form table, with fields for name, grade, sport, injured area, injured side, treatment type, athlete comments, trainer comments, and athlete status.
    Added a users table with fields for name, email, password (not encrypted, only shown as hashed_password_example), school name, and school code.
    Added an athlete's contacts table with fields for contact type and phone number.
    Added navigation to all our app's screens. 😀
    Created an athlete form file to store all the information of an athlete.
    Created an athlete contact file to store all the information of an athlete's contact.
    Got SchoolCodeScreen connected and functional with the database.
    Got AthleteInjuryForm connected and functional with the database. 😀😀😀😀😀
    Got UserCreateAccount connected and functional with the database. 😀😀😀
    Got TrainerSchoolInformation connected and functional with the database.
    Got AthleteStatuses connected and functional with the database. 😀😀
    Got AthleteContacts connected and functional with the database. 😀😀😀😀