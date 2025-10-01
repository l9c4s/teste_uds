FROM mariadb:11.4

# Definir variáveis de ambiente
ENV MYSQL_ROOT_PASSWORD=rootpassword
ENV MYSQL_DATABASE=usersdb
ENV MYSQL_USER=apiuser
ENV MYSQL_PASSWORD=apipassword

# Copiar scripts de inicialização se necessário
COPY init.sql /docker-entrypoint-initdb.d/

# Expor porta padrão do MariaDB
EXPOSE 3306

# Configurações adicionais do MariaDB
RUN echo '[mysqld]' >> /etc/mysql/conf.d/custom.cnf && \
    echo 'character-set-server=utf8mb4' >> /etc/mysql/conf.d/custom.cnf && \
    echo 'collation-server=utf8mb4_unicode_ci' >> /etc/mysql/conf.d/custom.cnf && \
    echo 'default-time-zone="+00:00"' >> /etc/mysql/conf.d/custom.cnf