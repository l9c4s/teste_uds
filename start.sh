#!/bin/bash
echo "🚀 Iniciando User Management System..."

# Build e start todos os serviços
docker-compose up --build -d

# Aguardar todos os serviços ficarem healthy
echo "⏳ Aguardando serviços ficarem prontos..."
docker-compose ps

echo "🔍 Verificando status dos serviços..."
echo "📊 Database: http://localhost:3306"
echo "🔧 API: http://localhost:5252/api/health"
echo "🌐 Frontend: http://localhost:3000"

echo "✅ Sistema iniciado com sucesso!"