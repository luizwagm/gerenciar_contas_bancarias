# Use the official Node.js image
FROM node:18-bullseye-slim AS build
WORKDIR /app

# Install Angular CLI
RUN npm install -g @angular/cli

# Copy package.json and package-lock.json and install dependencies
COPY package*.json ./

# Limpar o cache do npm e remover diretórios problemáticos antes de instalar dependências
RUN npm cache clean --force

# Tentar a instalação com repetição em caso de falha
RUN npm install --legacy-peer-deps || npm install --legacy-peer-deps || npm install --legacy-peer-deps

# Copy the rest of the application code
COPY . ./

# Expose the port Angular will serve on
EXPOSE 4201

# Start the Angular development server
CMD ["ng", "serve", "--host", "0.0.0.0", "--port", "4201"]
