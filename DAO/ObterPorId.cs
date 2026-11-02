using AppWebLuisGust.Configs;
using AppWebLuisGust.Model;
using Microsoft.AspNetCore.Razor.TagHelpers;
using MySql.Data.MySqlClient;
using System.Diagnostics;

public Processo? ObterPorId (int id)
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