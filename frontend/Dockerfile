# Używamy oficjalnego obrazu Node.js, aby zbudować aplikację React
FROM node:18 AS build
WORKDIR /app

# Skopiuj plik package.json i zainstaluj zależności
COPY package.json ./
COPY package-lock.json ./
RUN npm install

# Skopiuj wszystkie pliki źródłowe aplikacji i zbuduj aplikację
COPY . .

# Zbuduj aplikację produkcyjną (jeśli chcesz korzystać z trybu produkcyjnego, używaj tego kroku)
RUN npm run build

# Uruchom aplikację React w trybie development (użycie react-scripts start)
CMD ["npm", "start"]

# Domyślny port dla React App (używany przez npm start to 3000)
EXPOSE 3000
