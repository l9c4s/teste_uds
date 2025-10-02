# Estágio 1: Compilar a aplicação Angular
FROM node:20-alpine AS build
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
# O output path padrão para Angular 17+ é dist/<project-name>/browser
RUN npm run build -- --configuration production

# Estágio 2: Servir a aplicação com Nginx
FROM nginx:alpine
COPY --from=build /app/dist/*/browser /usr/share/nginx/html


RUN rm /etc/nginx/conf.d/default.conf
# Adiciona uma configuração customizada para suportar o roteamento do Angular
COPY nginx.conf /etc/nginx/conf.d

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]