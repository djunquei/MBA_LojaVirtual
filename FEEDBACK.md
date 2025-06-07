# Feedback - Avaliação Geral

## Front End

### Navegação
  * Pontos positivos:
    - Projeto MVC funcional com navegação completa e views organizadas para produtos, categorias e autenticação.

  * Pontos negativos:
    - Nenhum.

### Design
  - Interface simples, funcional e adequada ao contexto de administração de loja virtual.

### Funcionalidade
  * Pontos positivos:
    - CRUD de produtos e categorias implementado tanto na API quanto no MVC.
    - ASP.NET Identity implementado com autenticação via JWT (API) e Cookie (MVC).
    - Arquitetura enxuta, separada em três camadas: API, Data, UI.
    - SQLite, migrations automáticas e seed de dados corretamente aplicados.
    - Criação do vendedor junto com o usuário no MVC (ID compartilhado).

  * Pontos negativos:
    - API não implementa criação de usuário nem criação do vendedor associado ao Identity.
    - Ao criar produtos na API, o ID do vendedor não é extraído do usuário logado.
    - Permite que um vendedor edite produtos de outro, o que representa uma falha de segurança.

## Back End

### Arquitetura
  * Pontos positivos:
    - Projeto organizado em API, Data e MVC com clara separação de responsabilidades.
    - Configurações modulares com separação de contexto e DI.

  * Pontos negativos:
    - Implementação duplicada de `DbMigrationHelpers` tanto na API quanto no MVC, o que fere o princípio DRY.

### Funcionalidade
  * Pontos positivos:
    - Operações CRUD e autenticação funcionam corretamente em ambas as camadas.

  * Pontos negativos:
    - Faltam validações de integridade de domínio: associação entre produto e vendedor logado não é verificada.

### Modelagem
  * Pontos positivos:
    - Entidades bem estruturadas, relações claras e modelagem enxuta.

  * Pontos negativos:
    - Uso de `DataAnnotations` nas entidades ao invés da `Fluent API` do EF Core para mapeamentos.

## Projeto

### Organização
  * Pontos positivos:
    - Projeto estruturado com `src`, solution `.sln` na raiz, e boa separação entre camadas.
    - Documentação presente com `README.md` e `FEEDBACK.md`.

  * Pontos negativos:
    - Estrutura de seed repetida desnecessariamente.

### Documentação
  * Pontos positivos:
    - Documentação clara e com instruções de uso.
    - Swagger na API.

  * Pontos negativos:
    - Nenhum.

### Instalação
  * Pontos positivos:
    - Uso de SQLite com setup funcional e migrations automáticas.
    - Seed de dados funcionando no MVC.

  * Pontos negativos:
    - Seed de dados ausente na API.

---

# 📊 Matriz de Avaliação de Projetos

| **Critério**                   | **Peso** | **Nota** | **Resultado Ponderado**                  |
|-------------------------------|----------|----------|------------------------------------------|
| **Funcionalidade**            | 30%      | 9        | 2,7                                      |
| **Qualidade do Código**       | 20%      | 9        | 1,8                                      |
| **Eficiência e Desempenho**   | 20%      | 8        | 1,6                                      |
| **Inovação e Diferenciais**   | 10%      | 8        | 0,8                                      |
| **Documentação e Organização**| 10%      | 8        | 0,8                                      |
| **Resolução de Feedbacks**    | 10%      | 8        | 0,8                                      |
| **Total**                     | 100%     | -        | **8,5**                                  |

## 🎯 **Nota Final: 8,5 / 10**
