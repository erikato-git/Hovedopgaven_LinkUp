
## Intro:

LinkUp was an idea for a mobile application designed to connect entrepreneurs around projects. This project contains the source code and decissions made for the backend part. The idea for the application was never realized.

### Manual for using the LinkUp REST API:

[Manual in PDF](</Dokumenter/Manual til LinkUp REST-API.pdf>)

OBS: not completely updated but still useful.

<br>

## Domain classes and Architecutre:

![Model classes](/Dokumenter/image.png)

The model classes were derived from use cases based on the frontend part of LinkUp. The application is implemented following a Domain-Driven Architecture, where each domain class has an associated controller, service, and repository class, handling endpoints, business logic, and database transactions for the system, respectively.

![Architectural flow](/Dokumenter/image-2.png)

The architecture is implemented following the Onion Architecture, where each layer can only communicate with the layer below through one-way communication. Dependencies between layers are managed via interfaces, utilizing dependency inversion, where the concrete implementation of the interface is injected using dependency injection.

A repository layer has been added to allow for the desired implementation of a repository class for a given database. This enhances the system’s flexibility in terms of replacing the database in the long run and improves maintainability.


<br>

## Authentication and authorization:

Authentication is implemented using JWT authentication. When a user logs into their account, a JWT is generated with a claim containing the user's account ID. This ID is used in future requests to authorize access to the user's domains.

If the user wants to delete, create, or update their account or an associated profile, they are required to provide their password to ensure additional authentication.

### Security and OWASP:

The user's password is stored in the database after being salted with the user's account ID and hashed using the SHA-256 hashing algorithm.

A standard rate-limiting mechanism has been implemented on all endpoints to mitigate dictionary attacks. All input is sanitized through input DTOs, which validate user input.

Security measures for the system have been assessed according to "OWASP Top 10 API Security Risks - 2023".
[Link](https://owasp.org/API-Security/editions/2023/en/0x11-t10/)

Note: However, URL input sanitation to prevent SSRF (Server-Side Request Forgery) attacks has not been implemented (API7:2023). Additionally, input sanitation for scripts has not been implemented for either users or third-party services (API10:2023).

<br>

## GDPR and Cloudinary:

Since the system is intended as the backend for a social media app, it is also important to consider GDPR regulations regarding the processing of users' personal data.

To comply with legal requirements, measures have been implemented based on the Danish Data Protection Agency's guide: "7 Important Steps to GDPR Compliance". [Link](https://www.datatilsynet.dk/regler-og-vejledning/gdpr-univers-for-smaa-virksomheder)

### Challenges with GDPR in Relation to Cloudinary:

The intention with the app is for users to be able to upload videos and images of themselves and their projects on the platform. Since users can be identified through these media files, this falls under GDPR regulations regarding the processing of personal data.

It is planned that files will not be stored locally but instead on a cloud provider called Cloudinary, as the service is well-suited for startups, has high uptime, and includes a CDN solution.

However, using a hybrid cloud solution introduces challenges regarding data synchronization. If a user is deleted from the system's local database, their videos and images may remain stored in Cloudinary without a way to identify which media belongs to the deleted user.

The current implementation attempts to address this issue through a Media class, which contains a property storing the Cloudinary URL of the media file and the profile ID of the user who uploaded it. This allows the system to check whether a Media object contains a profile ID that no longer exists in the database. Based on this, media files in Cloudinary can be identified for deletion using their URL property.

However, this approach is manual and time-consuming, making it impractical for large-scale operations.

Additionally, when using third-party IT providers to process personal data, a data processing agreement (DPA) must be established with Cloudinary, and ongoing compliance monitoring of the provider must be conducted.

<br>

### WebApplicationFactory, Testcontainer and Integraion Tests:

The system's implementation has been carried out alongside extensive integration testing of its endpoints to ensure that the system consistently functions as intended, improves maintainability, and allows for refactoring during development.

WebApplicationFactory enables the use of an instance of the application for integration testing. Additionally, a Testcontainer can be configured for the specific database used in the system. This approach ensures that the system is continuously tested in an environment as close to production as possible, using a real database.

The Testcontainer automatically cleans up all allocated Docker volumes and networks after each test session, ensuring a reliable and consistent test environment every time the tests are executed.  

<br>

## Conclusion:

The system was developed as a final project for a company called LinkUp, which aimed to create the backend for a mobile application designed to connect entrepreneurs around projects on the platform.

While the system is not fully completed, it addresses many interesting aspects of architecture, authentication, security, GDPR compliance, and the use of external cloud services. These elements open discussions regarding the advantages and disadvantages of different implementation approaches.





























