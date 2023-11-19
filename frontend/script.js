const baseURL = 'http://localhost'; // ou substitua pelo endereço do seu servidor

async function listarAgendamentos() {
    const response = await fetch(`${baseURL}/agendamentos`);
    const data = await response.json();
    document.getElementById('resultadoListagemAgendamento').innerHTML = JSON.stringify(data);
}

async function listarPorMedico() {
    const idMedico = document.getElementById('idMedico').value;
    const response = await fetch(`${baseURL}/agendamento/medico/${idMedico}`);
    const data = await response.json();
    document.getElementById('resultadoPorMedico').innerHTML = JSON.stringify(data);
}

async function listarPorPaciente() {
    const idPaciente = document.getElementById('idPaciente').value;
    const response = await fetch(`${baseURL}/agendamento/paciente/${idPaciente}`);
    const data = await response.json();
    document.getElementById('resultadoPorPaciente').innerHTML = JSON.stringify(data);
}
async function listarEspecialidades() {
    const response = await fetch(`${baseURL}/especialidades`);
    const data = await response.json();
    document.getElementById('resultadoListagemEspecialidade').innerHTML = JSON.stringify(data);
}

async function listarPorEspecialidadeId() {
    const idEspecialidade = document.getElementById('idEspecialidade').value;
    const response = await fetch(`${baseURL}/especialidade/${idEspecialidade}`);
    const data = await response.json();
    document.getElementById('resultadoPorIdEspecialidade').innerHTML = JSON.stringify(data);
}

async function cadastrarEspecialidade() {
    const nomeEspecialidade = document.getElementById('nomeEspecialidade').value;
    const response = await fetch(`${baseURL}/especialidade`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ nome: nomeEspecialidade })
    });
    const data = await response.json();
    document.getElementById('resultadoCadastroEspecialidade').innerHTML = JSON.stringify(data);
}

async function atualizarEspecialidade() {
    const idAtualizar = document.getElementById('idAtualizar').value;
    const nomeAtualizado = document.getElementById('nomeAtualizado').value;
    const response = await fetch(`${baseURL}/especialidade/${idAtualizar}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ nome: nomeAtualizado })
    });
    const data = await response.json();
    document.getElementById('resultadoAtualizacaoEspecialidade').innerHTML = JSON.stringify(data);
}

async function deletarEspecialidade() {
    const idDeletar = document.getElementById('idDeletar').value;
    const response = await fetch(`${baseURL}/especialidade/${idDeletar}`, {
        method: 'DELETE'
    });
    const data = await response.json();
    document.getElementById('resultadoDelecaoEspecialidade').innerHTML = JSON.stringify(data);
}

async function cadastrarAgendamento() {
    const pacienteId = document.getElementById('pacienteId').value;
    const medicoId = document.getElementById('medicoId').value;
    const dataAgendamento = document.getElementById('dataAgendamento').value;

    const response = await fetch(`${baseURL}/agendamento`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            paciente: { id: parseInt(pacienteId) },
            medico: { id: parseInt(medicoId) },
            data: dataAgendamento,
        }),
    });

    const data = await response.json();
    document.getElementById('resultadoCadastroAgendamento').innerHTML = JSON.stringify(data);
}

async function deletarAgendamento() {
    const idAgendamentoDeletar = document.getElementById('idAgendamentoDeletar').value;

    const response = await fetch(`${baseURL}/agendamento/${idAgendamentoDeletar}`, {
        method: 'DELETE',
    });

    const data = await response.json();
    document.getElementById('resultadoDelecaoAgendamento').innerHTML = JSON.stringify(data);
}

async function listarMedicos() {
    const response = await fetch(`${baseURL}/medicos`);
    const data = await response.json();
    document.getElementById('listaMedicos').innerHTML = JSON.stringify(data);
}

async function buscarMedicoPorId() {
    const idMedico = document.getElementById('idMedico').value;
    console.log(idMedico)
    const response = await fetch(`${baseURL}/medico/${idMedico}`);
    const data = await response.json();
    document.getElementById('infoMedico').innerHTML = JSON.stringify(data);
}

async function cadastrarMedico() {
    const nomeMedico = document.getElementById('nomeMedico').value;
    const emailMedico = document.getElementById('emailMedico').value;
    const crmMedico = document.getElementById('crmMedico').value;
    const especialidadeId = document.getElementById('especialidadeId').value;
    console.log(especialidadeId)

    const response = await fetch(`${baseURL}/medico`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            nome: nomeMedico,
            email: emailMedico,
            crm: crmMedico,
            especialidade: { id: parseInt(especialidadeId) },
        }),
    });

    const data = await response.json();
    document.getElementById('resultadoCadastroMedico').innerHTML = JSON.stringify(data);
}

async function atualizarMedico() {
    const idMedicoAtualizar = document.getElementById('idMedicoAtualizar').value;
    const response = await fetch(`${baseURL}/medico/${idMedicoAtualizar}`);
    const medicoAtualizar = await response.json();

    const responseEspecialidades = await fetch(`${baseURL}/especialidades`);
    const especialidades = await responseEspecialidades.json();

    const especialidadeSelecionada = especialidades.find(e => e.id === medicoAtualizar.especialidade.id);

    document.getElementById('nomeMedico').value = medicoAtualizar.nome;
    document.getElementById('emailMedico').value = medicoAtualizar.email;
    document.getElementById('crmMedico').value = medicoAtualizar.crm;
    document.getElementById('especialidadeId').value = especialidadeSelecionada.id;

    const responseAtualizar = await fetch(`${baseURL}/medico/${idMedicoAtualizar}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            nome: document.getElementById('nomeMedico').value,
            email: document.getElementById('emailMedico').value,
            crm: document.getElementById('crmMedico').value,
            especialidade: { id: parseInt(document.getElementById('especialidadeId').value) },
        }),
    });

    const data = await responseAtualizar.json();
    document.getElementById('resultadoAtualizacaoMedico').innerHTML = JSON.stringify(data);
}

async function deletarMedico() {
    const idMedicoDeletar = document.getElementById('idMedicoDeletar').value;
    const response = await fetch(`${baseURL}/medico/${idMedicoDeletar}`, {
        method: 'DELETE',
    });
    const data = await response.json();
    document.getElementById('resultadoDelecaoMedico').innerHTML = JSON.stringify(data);
}

//-----------------------------------------------------------------------------------


async function listarPacientes() {
    const response = await fetch(`${baseURL}/pacientes`);
    const data = await response.json();
    document.getElementById('listaPacientes').innerHTML = JSON.stringify(data);
}

async function buscarPacientePorId() {
    const idPaciente = document.getElementById('idPaciente').value;
    const response = await fetch(`${baseURL}/paciente/${idPaciente}`);
    const data = await response.json();
    document.getElementById('infoPaciente').innerHTML = JSON.stringify(data);
}

async function cadastrarPaciente() {
    const nomePaciente = document.getElementById('nomePaciente').value;
    const emailPaciente = document.getElementById('emailPaciente').value;
    const cpfPaciente = document.getElementById('cpfPaciente').value;
    const enderecoPaciente = document.getElementById('enderecoPaciente').value;
    const planoId = document.getElementById('planoId').value;

    const response = await fetch(`${baseURL}/paciente`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            nome: nomePaciente,
            email: emailPaciente,
            cpf: cpfPaciente,
            endereco: enderecoPaciente,
            plano:{ id: parseInt(planoId) }
        }),
    });

    const data = await response.json();
    document.getElementById('resultadoCadastroPaciente').innerHTML = JSON.stringify(data);
}


async function atualizarPaciente() {
    const idPacienteAtualizar = document.getElementById('idPacienteAtualizar').value;
    const response = await fetch(`${baseURL}/paciente/${idPacienteAtualizar}`);
    const pacienteAtualizar = await response.json();

    const responsePlanos = await fetch(`${baseURL}/planos`);
    const planos = await responsePlanos.json();

    const planoSelecionado = planos.find(e => e.id === pacienteAtualizar.plano.id);

    document.getElementById('nomePaciente').value = pacienteAtualizar.nome;
    document.getElementById('emailPaciente').value = pacienteAtualizar.email;
    document.getElementById('cpfPaciente').value = pacienteAtualizar.cpf;
    document.getElementById('enderecoPaciente').value = pacienteAtualizar.endereco;
    document.getElementById('planoId').value = planoSelecionado.id;

    const responseAtualizar = await fetch(`${baseURL}/paciente/${idPacienteAtualizar}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            nome: document.getElementById('nomePaciente').value,
            email: document.getElementById('emailPaciente').value,
            cpf: document.getElementById('cpfPaciente').value,
            endereco: document.getElementById('enderecoPaciente').value,
            plano: { id: parseInt(document.getElementById('planoId').value) },
        }),
    });

    const data = await responseAtualizar.json();
    document.getElementById('resultadoAtualizacaoPaciente').innerHTML = JSON.stringify(data);
}

async function deletarPaciente() {
    const idPacienteDeletar = document.getElementById('idPacienteDeletar').value;
    const response = await fetch(`${baseURL}/paciente/${idPacienteDeletar}`, {
        method: 'DELETE',
    });
    const data = await response.json();
    document.getElementById('resultadoDelecaoPaciente').innerHTML = JSON.stringify(data);
}

//---------------------------------------------------------------------
async function listarPlanos() {
    const response = await fetch(`${baseURL}/planos`);
    const data = await response.json();
    document.getElementById('resultadoListagemPlano').innerHTML = JSON.stringify(data);
}

async function listarPorPlanoId() {
    const idPlano = document.getElementById('idPlano').value;
    const response = await fetch(`${baseURL}/plano/${idPlano}`);
    const data = await response.json();
    document.getElementById('resultadoPorIdPlano').innerHTML = JSON.stringify(data);
}

async function cadastrarPlano() {
    const convenioPlano = document.getElementById('convenioPlano').value;
    const descontoPlano = document.getElementById('descontoPlano').value;
    const response = await fetch(`${baseURL}/plano`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ convenio: convenioPlano , desconto: descontoPlano})
    });
    const data = await response.json();
    document.getElementById('resultadoCadastroPlano').innerHTML = JSON.stringify(data);
}

async function atualizarPlano() {
    const idAtualizar = document.getElementById('idAtualizar').value;
    const convenioAtualizado = document.getElementById('convenioAtualizado').value;
    const descontoAtualizado = document.getElementById('descontoAtualizado').value;
    const response = await fetch(`${baseURL}/plano/${idAtualizar}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ convenio: convenioAtualizado , desconto: descontoAtualizado})
    });
    const data = await response.json();
    document.getElementById('resultadoAtualizacaoPlano').innerHTML = JSON.stringify(data);
}

async function deletarPlano() {
    const idDeletar = document.getElementById('idDeletar').value;
    const response = await fetch(`${baseURL}/plano/${idDeletar}`, {
        method: 'DELETE'
    });
    const data = await response.json();
    document.getElementById('resultadoDelecaoPlano').innerHTML = JSON.stringify(data);
}