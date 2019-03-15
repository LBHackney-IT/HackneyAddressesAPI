# Hackney Addresses API (In development)

We will use this API when developing applications and services for the London Borough of Hackney. This ensures consistency and enables linking of data across the organisation (via the [Unique Property Reference Number](https://www.geoplace.co.uk/addresses/uprn)).

This API provides a limited interface to Hackney's [Local Land and Property Gazetteer](https://www.geoplace.co.uk/helpdesk/supporting-the-dca/address-custodians/importance-of-the-llpg) and to the [National Address Gazetteer](https://www.geoplace.co.uk/addresses/national-address-gazetteer).

## Stack

- .NET Core as a web framework.
- xUnit as a test framework.

## Dependencies

- SQL server database fed by property data GDS system

## Development practices

We employ a variant of Clean Architecture, borrowing from [Made Tech Flavoured Clean Architecture][mt-ca] and Made Tech's [.NET Clean Architecture library][dotnet-ca].

## Contributing
[TBC]
### Setup
[TBC]
### Development
[TBC]
### Release

We use a pull request workflow, where changes are made on a branch and approved by one or more other maintainers before the developer can merge into `master`.

Then we have an automated four step deployment process, which runs in CircleCI.

1. Automated tests (xUnit) are run to ensure the release is of good quality.
2. The app is deployed to staging automatically, where we check our latest changes work well.
3. We manually confirm a production deployment in the CircleCI workflow once we're happy with our changes in staging.
4. The app is deployed to production.

## Contacts

### Active Maintainers

- **Matthew Keyworth**, Lead Developer at London Borough of Hackney (matthew.keyworth@hackney.gov.uk)

### Other Contacts

[docker-download]: https://www.docker.com/products/docker-desktop
[mt-ca]: https://github.com/madetech/clean-architecture
[made-tech]: https://madetech.com/
[dotnet-ca]: https://github.com/madetech/dotnet-ca
