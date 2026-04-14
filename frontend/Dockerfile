FROM node:22-alpine AS build
WORKDIR /app

ARG REACT_APP_API_BASE_URL=http://localhost:5141/api
ENV REACT_APP_API_BASE_URL=$REACT_APP_API_BASE_URL

COPY package*.json ./
RUN npm ci

COPY . .
RUN npm run build

FROM nginx:alpine
COPY --from=build /app/build /usr/share/nginx/html
EXPOSE 80
