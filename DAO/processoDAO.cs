
using AppWebLuisGust.Configs;
using AppWebLuisGust.Model;
using MySql.Data.MySqlClient;

namespace AppWebLuisGust.DAO
{
    public class ProcessoDAO
    {
        private readonly Conexao _conexao;

        public ProcessoDAO(Conexao conexao)
        {
            _conexao = conexao;
        }

        // READ - Lista todos os processos
        public List<Processo> Listar()
        {
            var lista = new List<Processo>();

            using var con = _conexao.GetConnection();

            const string sql = "SELECT * FROM processos;";

            using var comando = con.CreateCommand();
            comando.CommandText = sql;

            using var leitor = comando.ExecuteReader();

            while (leitor.Read())
            {
                lista.Add(MapearProcesso(leitor));
            }

            return lista;
        }

        // CREATE - Cadastra um processo
        public void Inserir(Processo processo)
        {
            using var con = _conexao.GetConnection();

            const string sql = @"INSERT INTO processos
                (numero_pro, data_pro, interessado_pro,
                 assunto_pro, descricao_pro, situacao_pro)
                VALUES
                (@numero, @data, @interessado,
                 @assunto, @descricao, @situacao)";

            using var comando = con.CreateCommand();
            comando.CommandText = sql;

            comando.Parameters.AddWithValue("@numero", processo.Numero);
            comando.Parameters.AddWithValue("@data",
                processo.Data!.Value.ToDateTime(TimeOnly.MinValue));
            comando.Parameters.AddWithValue("@interessado", processo.Interessado);
            comando.Parameters.AddWithValue("@assunto", processo.Assunto);
            comando.Parameters.AddWithValue("@descricao", processo.Descricao);
            comando.Parameters.AddWithValue("@situacao", processo.Situacao);

            comando.ExecuteNonQuery();
        }

        // READ - Busca um processo pelo ID
        public Processo? ObterPorId(int id)
        {
            using var con = _conexao.GetConnection();

            const string sql = @"SELECT id_pro, numero_pro, data_pro,
                interessado_pro, assunto_pro, descricao_pro, situacao_pro
                FROM processos
                WHERE id_pro = @id";

            using var comando = con.CreateCommand();
            comando.CommandText = sql;
            comando.Parameters.AddWithValue("@id", id);

            using var leitor = comando.ExecuteReader();

            if (!leitor.Read())
            {
                return null;
            }

            return MapearProcesso(leitor);
        }

        // UPDATE - Atualiza um processo
        public void Atualizar(Processo processo)
        {
            using var con = _conexao.GetConnection();

            const string sql = @"UPDATE processos
                SET numero_pro = @numero,
                    data_pro = @data,
                    interessado_pro = @interessado,
                    assunto_pro = @assunto,
                    descricao_pro = @descricao,
                    situacao_pro = @situacao
                WHERE id_pro = @id";

            using var comando = con.CreateCommand();
            comando.CommandText = sql;

            comando.Parameters.AddWithValue("@numero", processo.Numero);
            comando.Parameters.AddWithValue("@data",
                processo.Data!.Value.ToDateTime(TimeOnly.MinValue));
            comando.Parameters.AddWithValue("@interessado", processo.Interessado);
            comando.Parameters.AddWithValue("@assunto", processo.Assunto);
            comando.Parameters.AddWithValue("@descricao", processo.Descricao);
            comando.Parameters.AddWithValue("@situacao", processo.Situacao);
            comando.Parameters.AddWithValue("@id", processo.Id);

            comando.ExecuteNonQuery();
        }

        // Converte os dados do banco para o objeto Processo
        private static Processo MapearProcesso(MySqlDataReader leitor)
        {
            DateOnly? data = null;

            if (!leitor.IsDBNull(leitor.GetOrdinal("data_pro")))
            {
                data = DateOnly.FromDateTime(
                    leitor.GetDateTime("data_pro"));
            }

            return new Processo
            {
                Id = leitor.GetInt32("id_pro"),
                Numero = DAOHelper.GetString(leitor, "numero_pro"),
                Data = data,
                Interessado = DAOHelper.GetString(leitor, "interessado_pro"),
                Assunto = DAOHelper.GetString(leitor, "assunto_pro"),
                Descricao = DAOHelper.GetString(leitor, "descricao_pro"),
                Situacao = DAOHelper.GetString(leitor, "situacao_pro")
            };
        }
    }
}
