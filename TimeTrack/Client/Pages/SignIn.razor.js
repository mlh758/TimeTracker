export async function makeAssertion(options) {
  options.challenge = fromBase64(options.challenge);
  options.allowCredentials.forEach((cred) => cred.id = fromBase64(cred.id));
  const result = await navigator.credentials.get({ publicKey: options });
  return encodeAttestation(result);
}

function encodeAttestation(att) {
  return {
    id: att.id,
    rawId: toBase64(att.rawId),
    type: att.type,
    response: {
      authenticatorData: toBase64(att.response.authenticatorData),
      clientDataJSON: toBase64(att.response.clientDataJSON),
      signature: toBase64(att.response.signature),
    },
    extensions: att.getClientExtensionResults(),
  }
}

/*
 * The C# code uses Base64UrlConverter to encode the bytes which comes with some quirks when encoding. Pulled this from
 * https://github.com/passwordless-lib/fido2-net-lib/blob/9823ec02991c4036f5d2f435e779044ff84ebab2/Demo/wwwroot/js/helpers.js#L1
 * To future travelers, I spent way more time dealing with []byte / string / ArrayBuffer nonsense than I did actually implementing this.
 */
function toBase64(thing) {
  // Array or ArrayBuffer to Uint8Array
  if (Array.isArray(thing)) {
    thing = Uint8Array.from(thing);
  }

  if (thing instanceof ArrayBuffer) {
    thing = new Uint8Array(thing);
  }

  // Uint8Array to base64
  if (thing instanceof Uint8Array) {
    var str = "";
    var len = thing.byteLength;

    for (var i = 0; i < len; i++) {
      str += String.fromCharCode(thing[i]);
    }
    thing = window.btoa(str);
  }

  if (typeof thing !== "string") {
    throw new Error("could not coerce to string");
  }

  // base64 to base64url
  // NOTE: "=" at the end of challenge is optional, strip it off here
  thing = thing.replace(/\+/g, "-").replace(/\//g, "_").replace(/=*$/g, "");

  return thing;
};


function fromBase64(str) {
  const byteCharacters = atob(str.replace(/-/g, "+").replace(/_/g, "/"));
  const byteNumbers = new Array(byteCharacters.length);
  for (var i = 0; i < byteCharacters.length; i++) {
    byteNumbers[i] = byteCharacters.charCodeAt(i);
  }
  return new Uint8Array(byteNumbers);
}