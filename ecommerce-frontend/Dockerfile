# Build aşaması
FROM node:20-alpine as build
WORKDIR /app

# .env dosyasını önceden kopyala ki build sırasında kullanılsın
COPY .env .env

COPY package.json package-lock.json ./
RUN npm install

COPY . .
RUN npm run build

# Production aşaması
FROM nginx:stable-alpine
COPY --from=build /app/build /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]

