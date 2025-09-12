FROM node:20 AS build
WORKDIR /app
COPY ./Beauty4u.Web ./Beauty4u.Web
WORKDIR /app/Beauty4u.Web
RUN npm install && npm run build -- --configuration production

FROM nginx:alpine
COPY --from=build /app/Beauty4u.Web/dist/beauty4u-web /usr/share/nginx/html
EXPOSE 80
