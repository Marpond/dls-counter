FROM node:latest AS build
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build || { echo "Build failed. Check esbuild configuration"; exit 1; }

FROM nginx:latest
COPY --from=build /app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
ENV NODE_ENV=production
WORKDIR /usr/share/nginx/html
CMD ["nginx", "-g", "daemon off;"]