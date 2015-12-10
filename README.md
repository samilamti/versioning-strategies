# Versioning is hard
... and the first rule of versioning is - "don't do versioning". When you need to deal with versioning your software components, 
however, here are some useful hints and tips.

To follow the scenarios, clone the repository and checkout branches for each step:

    git checkout -b Example0 ReleaseN+0

- Open each solution (`Consumer\Consumer.sln`, `Producer\Producer.sln` and `MessageContracts\MessageContracts.sln`).
- Rebuild the solutions as needed (if you're unsure, just rebuild them all).
- Test compatibility using the `test.ps1` PowerShell script (which just fires up the Produer and Consumer in succession).

The `MessageContracts` solution produces a [semantically versioned](http://semver.org/) NuGet package. To build `MessagingContracts`
after a checkout and generate said library, run the `build.ps1` PowerShell script in its folder. It will pressume that you have a
`..\lib` directory and that **you have mapped this directory as a local NuGet repository** (`Tools -> Options -> NuGet Package 
Manager -> Package Sources`).

## Scenario 1 – tag `ReleaseN+0`
This is the initial release. Unsurprisingly, everything works just fine.
- Contracts, Producer and Consumer are in version 1.0.0.

## Scenario 2 – tag `ReleaseN+1`
Things are getting interesting - we want to make changes and new data is flowing through the system. But the consuming service 
won't have time to implement the new functionality in this release!
- *Message Contracts*: New features (future changes) are introduced; properties-to-be-removed are flagged as `[Obsolete]`. **Release v1.1.0**.
- *Producer* takes a dependency to the new contracts and implements the new functionality. **Is released**.
- *Consumer* does nothing.

## Scenario 3 – tag `ReleaseN+2`
- *Consumer* catches up by taking a dependency on version 1.1.0 of the Message Contracts. It deals with the obsoletion warnings, depending only on the new / changed properties. **Is released**.
- *Message Contracts*: The obsoleted properties are removed. **Release v2.0.0**.
- *Producer* takes a dependency on version 2.0.0 and performs the necessary housekeeping, removing obsoleted properties. **Is Released**.

## Scenario 4 - tag `ReleaseN+3`
- *Consumer* again catches up by taking a dependency on the latest version of Message Contracts - v2.0.0. This time, the consumer needs to do nothing more - all necessary changes was done during the last release!

***Message Contracts, Producer and Consumer are all in sync with no breaking changes!***
