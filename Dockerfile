FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
ARG BUILDPLATFORM
ARG TARGETARCH
ARG TARGETPLATFORM
RUN echo "Running on $BUILDPLATFORM, building for $TARGETPLATFORM"

WORKDIR /samples

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -a $TARGETARCH -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/runtime:8.0
USER app
WORKDIR /samples
COPY --from=build-env /samples/out .
ENTRYPOINT ["./samples"]
