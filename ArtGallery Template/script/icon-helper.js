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
      }else{
          reject()
      }
  })
};

async function buf2hex(blob) { // buffer is an ArrayBuffer
  let buffer = await blob.arrayBuffer();
  return [...new Uint8Array(buffer)]
      .map(x => x.toString(16).padStart(2, '0'))
      .join('');
}

async function getScoreByTX(txHash){
  return new Promise((resolve,reject)=>{
      setTimeout(async ()=>{
          try{
              const transaction = {
                  "jsonrpc" : "2.0",
                  "method": "icx_getTransactionResult",
                  "id": 2,
                  "params": {
                      "txHash": txHash
                  }
              }
              let txRes = await sendTransaction(transaction);
              resolve(txRes.result.scoreAddress)
          }catch(e){
              reject(e)
              console.error(e)
          }
      },5000);
  })
}

const deplyContract = async (symbol, title, uri)=>{
  
  let address = await connectWallet();
  console.log(address)
  let r = await fetch('metaverse.jar')
  let content = '0x'+await buf2hex(await r.blob())
  let txObj = new IconService.IconBuilder.DeployTransactionBuilder()
                  .from(address)
                  .to('cx0000000000000000000000000000000000000000')
                  .stepLimit(IconService.IconConverter.toBigNumber(10000000000))
                  .nid(IconService.IconConverter.toBigNumber(2))
                  .nonce(IconService.IconConverter.toBigNumber(1))
                  .version(IconService.IconConverter.toBigNumber(3))
                  .timestamp(new Date().getTime() * 1000)
                  .contentType('application/java')
                  .content(content)
                  .params({
                      _Uri: uri,
                      _name: title,
                      _symbol: symbol,
                  })
                  .build()

  const transaction = {
      jsonrpc: '2.0',
      method: 'icx_sendTransaction',
      params: IconService.IconConverter.toRawTransaction(txObj),
      id: 6339
  };
  let txRes = await sendTransaction(transaction);
  console.log(txRes.result)

  let scoreAddr = await getScoreByTX(txRes.result);
  console.log(scoreAddr)
  return scoreAddr;
}
