# -------------------------
# Stage 1: Build
# -------------------------
FROM node:22 AS build

WORKDIR /app

# Install dependencies
COPY package*.json ./
RUN npm install

# Copy source and build
COPY . .
RUN npm run build -- --configuration production

# -------------------------
# Stage 2: Serve via Nginx
# -------------------------
FROM nginx:alpine

# Copy built files to nginx
COPY --from=build /app/dist/beauty4u-web/browser /usr/share/nginx/html

# Expose default HTTP port
EXPOSE 80

# Start Nginx
CMD ["nginx", "-g", "daemon off;"]
