FROM node:22 AS build
WORKDIR /app
COPY . .
RUN npm ci
RUN npm run build -- --configuration=production

FROM nginx:alpine
COPY --from=build /app/dist/beauty4u-web /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf

COPY docker-entrypoint.sh /docker-entrypoint.sh
RUN chmod +x /docker-entrypoint.sh
ENTRYPOINT ["/docker-entrypoint.sh"]

CMD ["nginx", "-g", "daemon off;"]
