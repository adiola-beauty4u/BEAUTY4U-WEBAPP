FROM node:22 AS build
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build --legacy-peer-deps -- --configuration production

FROM nginx:alpine
COPY --from=build /app/dist/beauty4u-web /usr/share/nginx/html
EXPOSE 80
