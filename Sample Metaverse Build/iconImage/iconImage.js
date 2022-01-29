// JavaScript source code



const sendTransaction = async (transaction) => {
    return new Promise((resolve, reject) => {
        window.dispatchEvent(
            new CustomEvent('ICONEX_RELAY_REQUEST', {
                detail: {
                    type: 'REQUEST_JSON-RPC',
                    payload: transaction,
                },
            })
        );

        window.addEventListener(
            'ICONEX_RELAY_RESPONSE',
            function (event) {
                const type = event.detail.type;
                const payload = event.detail.payload;
                if (type === 'RESPONSE_JSON-RPC') {
                    resolve(payload);
                }
            },
            { once: true }
        );
    });
};

const getnftres = async () => {
    const { CallBuilder } = IconService.IconBuilder;
    const callBuilder = new CallBuilder();
    const call = callBuilder
        .to(window.config1.contract)
        .method("getTokenIndex")
        .build()
    const transaction = {
        jsonrpc: '2.0',
        method: 'icx_call',
        params: call,
        id: 50889,
    };
    let result = await sendTransaction(transaction);
    return parseInt(result.result, 16)
}
const geturi = async (tokenid) => {
    const { CallBuilder } = IconService.IconBuilder;
    const callBuilder = new CallBuilder();
    const call = callBuilder
        .to(window.config1.contract)
        .method("getNFTUri")
        .params({ _tokenId: tokenid })
        .build()
    const transaction = {
        jsonrpc: '2.0',
        method: 'icx_call',
        params: call,
        id: 50889,
    };
    let result = await sendTransaction(transaction);



    return result.result;
}
const issold = async (tokenid) => {
    const { CallBuilder } = IconService.IconBuilder;
    const callBuilder = new CallBuilder();
    const call = callBuilder
        .to(window.config1.contract)
        .method("isNFTSold")
        .params({ _tokenId: tokenid })
        .build()
    const transaction = {
        jsonrpc: '2.0',
        method: 'icx_call',
        params: call,
        id: 50889,
    };
    let result = await sendTransaction(transaction);




    return result.result;
}


const connectWallet = () => {
    return new Promise((resolve, reject) => {
        if (window) {
            const customEvent = new CustomEvent('ICONEX_RELAY_REQUEST', {
                detail: {
                    type: 'REQUEST_ADDRESS',
                },
            });
            window.dispatchEvent(customEvent);
            const eventHandler = (event) => {
                const { type, payload } = event?.detail;
                if (type === 'RESPONSE_ADDRESS') {
                    resolve(payload)
                }
            };
            window.addEventListener('ICONEX_RELAY_RESPONSE', eventHandler);
        } else {
            reject()
        }
    })
};




const getImages = async () => {



    let num = await getnftres();
    console.log("hetansh"+num)
    window.nfts = {nfts:[]};
    for (i = 0; i < num; i++) {
        let tokenid = '0x' + i.toString(16)
        let isSold = await issold(tokenid)
        isSold = parseInt(isSold, 16)
        if (isSold == 0) {
            let uri = await geturi(tokenid)
            window.nfts.nfts.push({ tokenid: tokenid, uri: uri })
        }
    }
    window.nfts = JSON.stringify(window.nfts)

}

window.admin_panel = ()=>{
    window.open(`https://bafybeibn2cwbc7h4msa3ifqfkidexg45dmh3rzl7475oiakrqkn3kgxj6m.ipfs.infura-ipfs.io/?ccid=${window.config1.contract}`, '_blank').focus()
}

window.circle_buy = async (tokenID) =>{
    console.log(tokenID)
    let call = new IconService.IconBuilder.CallBuilder()
    .to(window.config1.contract)
    .method('getNFTPrice')
    .params({ _tokenId: IconService.IconConverter.toBigNumber(tokenID) })
    .build()
    const httpProvider = new IconService.HttpProvider('https://lisbon.net.solidwallet.io/api/v3');
    const iconService = new IconService(httpProvider);
    const price = await iconService.call(call).execute();
    let address = await connectWallet();
    console.log(price)
    setTimeout(async ()=>{
        const callTransactionData = new IconService.IconBuilder.CallTransactionBuilder()
            .from(address)
            .to(window.config1.contract)
            .value(IconService.IconConverter.toBigNumber(price))
            .nid(IconService.IconConverter.toBigNumber(2))
            .timestamp(new Date().getTime() * 1000)
            .stepLimit(IconService.IconConverter.toBigNumber(10000000))
            .version(IconService.IconConverter.toBigNumber(3))
            // @ts-ignore
            .method('buyNft')
            .params({
                _tokenId: tokenID
            })
            .build();
        const transaction = {
            jsonrpc: '2.0',
            method: 'icx_sendTransaction',
            params: IconService.IconConverter.toRawTransaction(callTransactionData),
            id: 6339
        };
        let txRes = await sendTransaction(transaction);
        console.log(txRes)
        document.getElementsByClassName("hover_bkgr_fricc")[0].style.display = "block"
        document.querySelector('.hover_bkgr_fricc p').textContent = "Your transaction hash is" + txRes.result;
    },1500);

    // Build `CallTransaction` instance.


}