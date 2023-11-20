var url = 'http://localhost:80/'

function cadastrar(idDocumento, endpoint) {
	let divNome = document.getElementById(idDocumento)
	let childs = divNome.children;
	const eventoBlur = new FocusEvent('blur');
	for (let i = 0; i < childs.length; i++) {
		childs.item(i).dispatchEvent(eventoBlur);
	}
	for (let i = 0; i < childs.length; i++) {
		if ((childs.item(i).classList).contains("erro-input")) {
			return;
		}
	}

	let body = {};
	let time;
	for (let i = 0; i < childs.length; i++) {
		let child = childs.item(i);
		let key = child.id.substring(child.id.indexOf("_") + 1)
		if (key.length > 0) {
			if (key.indexOf("select") >= 0) {
				key = key.substring(0, key.indexOf("_"));
				body[key] = { id: parseInt(child.value) }
			} else if (key.indexOf("time") >= 0) {
				time = child.value;
			} else {
				body[key] = child.value
			}
		}
	}
	body[data] = body[data] + "T" + time;
	//envio da requisicao usando a FETCH API

	//configuracao e realizacao do POST no endpoint "usuarios"
	fetch(url + endpoint,
		{
			'method': 'POST',
			'headers':
			{
				'Content-Type': 'application/json',
				'Accept': 'application/json'
			},
			'body': JSON.stringify(body)
		})
		.then((response) => {
			if (response.ok) {
				return response.text()
			}
			else {
				return response.text().then((text) => {
					throw new Error(text)
				})
			}
		})
		.then((output) => {
			console.log(output)
			alert('Cadastro efetuado! :D')
			return;
		})
		.catch((error) => {
			console.log(error)
			alert(error)
		})
}

function validaData(doc) {
	if (doc.value.length > 0) {
		removerClassErro(doc.getAttribute("id"));
		return true
	}
	else {
		adicionarClassErro(doc.getAttribute("id"));
		return false
	}
}

function validaHora(doc) {
	if (doc.value.length > 0) {
		removerClassErro(doc.getAttribute("id"));
		return true
	}
	else {
		adicionarClassErro(doc.getAttribute("id"));
		return false
	}
}

function adicionarClassErro(id) {
	let divNome = document.getElementById(id)
	if (!divNome.classList.contains('erro-input')) {
		divNome.classList.add('erro-input')
	}
}

function removerClassErro(id) {
	let divNome = document.getElementById(id)
	divNome.classList.remove('erro-input')
}

function validaNome(id) {
	let divNome = document.getElementById(id)
	if (divNome.value.trim().split(' ').length >= 2) {
		divNome.classList.remove('erro-input')
		return true
	}
	else {
		if (!divNome.classList.contains('erro-input')) {
			divNome.classList.add('erro-input')
		}
		return false
	}
}

function validaEmail(id) {
	let divData = document.getElementById(id)
	if (
		divData.value.indexOf("@") >= 0
		&& (
			divData.value.indexOf(".com") >= 0
			|| divData.value.indexOf(".br") >= 0
		)
	) {
		removerClassErro(id);
		return true
	}
	else {
		adicionarClassErro(id);
		return false
	}
}

function validaCPF(id) {
	let divData = document.getElementById(id)
	if (divData.value.length > 0) {
		removerClassErro(id);
		return true
	}
	else {
		adicionarClassErro(id);
		return false
	}
}

function validaEndereco(id) {
	let divData = document.getElementById(id)
	if (divData.value.length > 0) {
		removerClassErro(id);
		return true
	}
	else {
		adicionarClassErro(id);
		return false
	}
}


function formatarMask(mascara, documento) {
	let i = documento.value.length;
	let saida = '#';
	let texto = mascara.substring(i);
	while (texto.substring(0, 1) != saida && texto.length) {
		documento.value += texto.substring(0, 1);
		i++;
		texto = mascara.substring(i);
	}
}

function listar(idDocumento, endpoint) {
	fetch(url + endpoint)
		.then(response => response.json())
		.then(async (entidades) => {

			let listaPacientes = document.getElementById(idDocumento)

			while (listaPacientes.firstChild) {
				listaPacientes.removeChild(listaPacientes.firstChild)
			}

			for (let entidade of entidades) {
				let divPaciente = document.createElement('div')
				divPaciente.setAttribute("id", entidade["id"])
				for (let key in entidade) {
					let val = entidade[key]
					if (typeof (val) != "object" && val != null) {
						let div = document.createElement('input')
						if (key == "ativo") {
							div.type = "checkbox"
							div.checked = val;
						}
						div.placeholder = key
						div.value = val
						divPaciente.appendChild(div)
					} else {
						let planos = await fetch(url + key + "s").then(response => response.json()).then(async (dados) => { return dados; })
						let sel = document.createElement("select")
						sel.setAttribute("name", key);

						for (let element of planos) {
							let optElement = document.createElement('option')
							let tempInnerHTML;
							if (element.nome != null) {
								tempInnerHTML = element.nome;
							} else if (element.convenio != null) {
								tempInnerHTML = element.convenio;
							} else if (element.data != null) {
								tempInnerHTML = element.data;
							} else {
								tempInnerHTML = "????";
							}
							optElement.innerHTML = tempInnerHTML
							optElement.value = element.id
							optElement.selected = element.id == val.id;
							sel.appendChild(optElement)
						}
						divPaciente.appendChild(sel);
					}
				}
				let butaoRemover = document.createElement("button")
				butaoRemover.textContent = "Desativar";
				butaoRemover.addEventListener("click", (event) => { remover(entidade["id"], endpoint, idDocumento) });
				divPaciente.appendChild(butaoRemover);

				let butaoAtualizar = document.createElement("button")
				butaoAtualizar.textContent = "Atualizar";
				butaoAtualizar.addEventListener("click", (event) => { atualizar(entidade["id"], endpoint, idDocumento) });
				divPaciente.appendChild(butaoAtualizar);
				listaPacientes.appendChild(divPaciente)
			}
		})
}

function getListaEntidades(endpoint, idElement) {
	fetch(url + endpoint)
		.then(response => response.json())
		.then((elements) => {
			let sel = document.getElementById(idElement)

			for (let element of elements) {
				let optElement = document.createElement('option')
				let tempInnerHTML;
				if (element.nome != null) {
					tempInnerHTML = element.nome;
				} else if (element.convenio != null) {
					tempInnerHTML = element.convenio;
				} else if (element.data != null) {
					tempInnerHTML = element.data;
				} else {
					tempInnerHTML = "????";
				}
				optElement.innerHTML = tempInnerHTML
				optElement.value = element.id
				sel.appendChild(optElement)
			}
		})
}

function atualizar(idEntidade, endpoint, idDocumento) {
	console.log(`idEntidade:${idEntidade} endpoint:${endpoint} idDocumento:${idDocumento}`);

	let listaEntidade = document.getElementById(idDocumento).children

	let dadosEntidade = null;
	let body = {};

	for (let i = 0; i < listaEntidade.length; i++) {
		let entidade = listaEntidade.item(i);
		if (entidade.getAttribute("id") == idEntidade) {
			dadosEntidade = entidade.children;
			break;
		}
	}

	if (dadosEntidade === null) {
		return;
	}

	for (let i = 0; i < dadosEntidade.length; i++) {
		let dado = dadosEntidade.item(i);
		if (dado.tagName != "BUTTON") {
			let key;
			if (dado.getAttribute("type") == "checkbox") {
				key = dado.getAttribute("placeholder");
				body[key] = dado.checked;
			} else if (dado.getAttribute("placeholder") != null) {
				key = dado.getAttribute("placeholder");
				body[key] = dado.value
			} else {
				key = dado.getAttribute("name")
				body[key] = { id: parseInt(dado.value) }
			}
		}
	}

	delete body["id"];

	if (endpoint.endsWith("s")) {
		endpoint = endpoint.substring(0, endpoint.length - 1);
	}

	console.log(body)


	fetch(url + endpoint + "/" + idEntidade,
		{
			'method': 'PUT',
			'headers':
			{
				'Content-Type': 'application/json',
				'Accept': 'application/json'
			},
			'body': JSON.stringify(body)
		})
		.then((response) => {
			if (response.ok) {
				return response.text()
			}
			else {
				return response.text().then((text) => {
					console.log(text)
					throw new Error(text)
				})
			}
		})
		.then((output) => {
			console.log(output)
			alert(output)
		})
		.catch((error) => {
			console.log(error)
			alert(error)
		})

}

function remover(idEntidade, endpoint, idDocumento) {
	if (endpoint.endsWith("s")) {
		endpoint = endpoint.substring(0, endpoint.length - 1);
	}
	fetch(url + endpoint + '/' + idEntidade,
		{
			'method': 'DELETE'
		})
		.then((response) => {
			if (response.ok) {
				return response.text()
			}
			else {
				return response.text().then((text) => {
					throw new Error(text)
				})
			}
		})
		.then((output) => {
			console.log(output)
			alert("desativado com sucesso")
		})
		.catch((error) => {
			alert(error)
		})
}
