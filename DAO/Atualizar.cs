using AppWebLuisGust.Configs;
using AppWebLuisGust.Model;
using Microsoft.AspNetCore.Razor.TagHelpers;
using MySql.Data.MySqlClient;
using System.Diagnostics;

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