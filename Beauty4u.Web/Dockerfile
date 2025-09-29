# -------------------------
# Stage 1: Build
# -------------------------
FROM node:22 AS build

WORKDIR /app

COPY package*.json ./
RUN npm install

COPY . .
RUN npm run build -- --configuration production

# -------------------------
# Stage 2: Serve via Nginx
# -------------------------
FROM nginx:alpine

# Copy built Angular app
COPY --from=build /app/dist/beauty4u_web/browser /usr/share/nginx/html

# Replace default nginx config
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
