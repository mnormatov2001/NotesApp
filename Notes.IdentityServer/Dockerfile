FROM bitnami/dotnet-sdk:6.0.416 AS builder
EXPOSE 7291
EXPOSE 5284

COPY . /bitnami/www/Notes.IdentityServer/
WORKDIR /bitnami/www/Notes.IdentityServer
RUN dotnet publish Notes.IdentityServer.csproj -o /bitnami/www/out/IdentityServer/ \
    -c Release --use-current-runtime --self-contained true

FROM ubuntu:latest

COPY --from=builder /bitnami/www/out/IdentityServer /root/IdentityServer

WORKDIR /root/ssl

ARG SMTP_CLIENT_USERNAME
ARG SMTP_CLIENT_PASSWORD
ARG HOSTNAME=auth.notes.com
ARG LOCALHOST_ROOT_CA_CRT
ARG LOCALHOST_ROOT_CA_KEY
ARG LOCALHOST_ROOT_CA_PASSWORD
ARG LOCALHOST_CERT_PASSWORD=localhost-cert-password

RUN apt update && apt upgrade -y && \
    apt install openssl -y ca-certificates -y && \
    \
    echo "${LOCALHOST_ROOT_CA_CRT}" >> root_ca.crt && \
    echo "${LOCALHOST_ROOT_CA_KEY}" >> root_ca.key && \
    \
    openssl genrsa -out localhost.key && \
    \
    openssl req -new -key localhost.key -out localhost.csr -batch \
    -subj "/C=RU/ST=Moscow/L=Moscow/O=Notes/CN=localhost" && \
    \
    printf "authorityKeyIdentifier=keyid,issuer \
    \nbasicConstraints=CA:FALSE \
    \nsubjectAltName=@alt_names \
    \n[alt_names] \
    \nDNS.1=localhost \
    \nDNS.2=${HOSTNAME} \
    \nDNS.3=host.docker.internal \
    \nIP.1=127.0.0.1" >> localhost.ext && \
    \
    openssl x509 -req -CA root_ca.crt -CAkey root_ca.key -in localhost.csr \
    -out localhost.crt -days 1826 -CAcreateserial -extfile localhost.ext -passin pass:"${LOCALHOST_ROOT_CA_PASSWORD}" && \
    \
    cat localhost.crt root_ca.crt > localhost_full.crt && \
    \
    openssl pkcs8 -inform PEM -topk8 -in localhost.key -out localhost.pem -passout pass:"${LOCALHOST_CERT_PASSWORD}" && \
    \
    cp root_ca.crt /usr/share/ca-certificates/ && \
    cp root_ca.crt /usr/local/share/ca-certificates/ && \
    \
    update-ca-certificates && \
    \
    cp localhost.crt /etc/ssl/certs/ && \
    cp localhost.pem /etc/ssl/private/

ENV ASPNETCORE_ENVIRONMENT=Development
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1
ENV Kestrel__Certificates__Default__Path=/etc/ssl/certs/localhost.crt
ENV Kestrel__Certificates__Default__KeyPath=/etc/ssl/private/localhost.pem
ENV Kestrel__Certificates__Default__Password="${LOCALHOST_CERT_PASSWORD}"
ENV SmtpClient__Username=${SMTP_CLIENT_USERNAME}
ENV SmtpClient__Password=${SMTP_CLIENT_PASSWORD}

WORKDIR /root/IdentityServer/

ENTRYPOINT ["./Notes.IdentityServer"]
