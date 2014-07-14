Imports Fretala
Imports System.Collections
Module Example

    Private Property Api As Object
    Private Property CostResult As String

    Private Property FreteResult As Dictionary(Of String, Object)

    Sub Main()
        Dim Auth As New Dictionary(Of String, String)
        Auth.Add("clientId", "ecommerce")
        Auth.Add("clientSecret", "Q6eH4nxD")
        Auth.Add("username", "jonathan@uecommerce.com.br")
        Auth.Add("password", "5X7uT[INU0oC")
        Api = New Fretala.Api("sandbox", Auth)

        Dim CostExample = New With {
            .from = New With {
                .number = "234",
                .street = "Rua Rio de Janeiro 653",
                .city = "Belo Horizonte",
                .state = "Minas Gerais"
            },
        .to = "30140-061"
        }

        Dim FreteExample = New With {
            .id = "MM8513110213",
            .productValue = "6000",
            .from = New With {
                .number = "234",
                .street = "Rua Rio de Janeiro",
                .city = "Belo Horizonte",
                .state = "Minas Gerais"
             },
            .to = New With {
                .number = "2500",
                .street = "Rua Timbiras 2500",
                .city = "Belo Horizonte",
                .state = "Minas Gerais"
             }
        }

        CostResult = Api.cost(CostExample)
        FreteResult = Api.insertFrete(FreteExample)
        Console.Write(CostResult)
        Console.Write(FreteResult.Item("_id"))
    End Sub

End Module