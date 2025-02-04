# TestTrailidentity
This repository contains the **TasteTrailIdentity** microservice, a foundational component of the **TasteTrail** application. It handles user authentication, authorization, and identity management by issuing secure JWT tokens and enabling users to manage their account details.  

## Features:
- **User Authentication**: Secure login and registration processes with password hashing.  
- **JWT Token Issuance**: Generates and validates JSON Web Tokens (JWT) for authenticated access to the platform.  
- **User Profile Management**: Allows users to update account details, including name, email, and password.  
- **Security Best Practices**: Implements safeguards such as token expiration and refresh tokens.

## Tech Stack:
- **Framework**: ASP.NET Core Web API  
- **Database**: PostgreSQL  
- **Authentication**: JWT and AspIdentity
- **Storage**: Azure Blob Storage (for user avatars)
- **Hosting**: Docker-ready for deployment in microservice environments  
