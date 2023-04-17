# Base image
FROM node:16-alpine AS build
WORKDIR /app

# Copy package.json and package-lock.json to container
COPY Builders.Bills.Web/package*.json ./

COPY Web.nginx.conf ./nginx.conf 

# Install npm packages
RUN npm install

# Copy Angular app files to container
COPY Builders.Bills.Web/ .

# Build Angular app
RUN npm run build --prod

# Stage 2: Run app using Nginx
FROM nginx:alpine
COPY --from=build /app/dist/builders.bills.web /usr/share/nginx/html
COPY --from=build app/nginx.conf /etc/nginx/conf.d/default.conf 
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]