using Microsoft.AspNetCore.Authorization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WebApplication2;

public class SoapSimple 
{
    private readonly HttpClient _httpClient;

    public SoapSimple()
    {
        // Configurar HttpClientHandler para autenticación NTLM
        var handler = new HttpClientHandler
        {
            Credentials = new NetworkCredential("aspnet", "Usuario1$", "etlvm"), 
            PreAuthenticate = true,
            UseDefaultCredentials = false,
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator

        };
        //handler.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
        // Usar el AuthorizationHandler como DelegatingHandler

        // Crear HttpClient con el handler configurado
        _httpClient = new HttpClient(handler);
    }

    public async Task<string> Send ()
    {
        // SOAP Message
        string soapMessage = GetSoapMessage();
        // SOAP Request
        var soapRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new System.Uri("https://etlvm.westeurope.cloudapp.azure.com/horizontest/TradeExecutionWS.asmx"), // Cambia esto por la URL de tu servicio SOAP
            Content = new StringContent(soapMessage, Encoding.UTF8, "text/xml"),
            

        };
        // Agregar el encabezado "Authorization"
        if (!soapRequest.Headers.Contains("Authorization"))
        {
            soapRequest.Headers.Add("Authorization", "Negotiate ");
        }
        // Set SOAP Headers
        soapRequest.Headers.Add("SOAPAction", "http://allegrodevelopment.com/UpdateTradeExecution"); // Cambia esto por tu acción SOAP

        //request.Properties[HttpRequestMessageProperty.Name] = httpRequestMessageProperty;

        string soapResponse = null;

        try
        {
            HttpResponseMessage response = _httpClient.Send(soapRequest);

            //soapRequest.Headers.Authorization.Parameter = "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes("aspnet:Usuario1$"));

            if (response.IsSuccessStatusCode)
            {
                soapResponse = "OK ";
            }
            else
            {
                soapResponse = "Error ";
            }

            soapResponse += await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException ex)
        {
            soapResponse = " Error " + ex.Message + " " +ex.StackTrace;
        }

        return soapResponse;
    }

    public string GetSoapMessage()
    {
        string soapMessage = @"<?xml version=""1.0"" encoding=""utf-8""?>
<soap12:Envelope xmlns:soap12=""http://www.w3.org/2003/05/soap-envelope"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
	<soap12:Body>
		<UpdateTradeExecution xmlns=""http://allegrodevelopment.com/"">
			<tradeExecutionDS>
				<xs:schema id=""TradeExecutionUIDS"" targetNamespace=""http://allegrodevelopment.com/TradeExecutionUIDS.xsd"" xmlns:mstns=""http://allegrodevelopment.com/TradeExecutionUIDS.xsd"" xmlns=""http://allegrodevelopment.com/TradeExecutionUIDS.xsd"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"" xmlns:msdata=""urn:schemas-microsoft-com:xml-msdata"" xmlns:msprop=""urn:schemas-microsoft-com:xml-msprop"" attributeFormDefault=""qualified"" elementFormDefault=""qualified"">
					<xs:element name=""TradeExecutionUIDS"" msdata:IsDataSet=""true"" msdata:UseCurrentLocale=""true"" msdata:EnforceConstraints=""False"" msprop:viewname=""Trade Execution"" msprop:AuditAsmx=""TradeExecution/TradeExecutionWS.asmx"" msprop:classname=""TradeExecution"" msprop:classtype=""Transaction"">
						<xs:complexType>
							<xs:choice minOccurs=""0"" maxOccurs=""unbounded"">
								<xs:element name=""trade"" msprop:InRefreshingDrillTables=""False"" msprop:ActiveRowIndex=""0"" msprop:panevisible=""True"" msprop:ProxyVersion=""v2"" msprop:viewpane=""trade"">
									<xs:complexType>
										<xs:sequence>
											<xs:element name=""trade"" msdata:Caption=""Trade"" msprop:dbcolumn=""trade"" msprop:expression="""" msprop:dbtable=""trade"" msprop:tempsysgen=""0"" default="""">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""tradetype"" msdata:Caption=""Trade type"" msprop:dbcolumn=""tradetype"" msprop:expression="""" msprop:dbtable=""trade"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""tradedate"" msdata:Caption=""Trade date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""tradedate"" msprop:trade_x003A_columnformat=""ShortDateTime"" msprop:expression="""" msprop:dbtable=""trade"" type=""xs:dateTime"" default=""2024-11-18T07:55:14.6445014"" minOccurs=""0""/>
											<xs:element name=""company"" msdata:Caption=""Company"" msprop:dbcolumn=""company"" msprop:expression="""" msprop:viewdefault=""VISTA"" msprop:dbtable=""trade"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""timeperiod"" msdata:Caption=""Time period"" msprop:dbcolumn=""timeperiod"" msprop:expression="""" msprop:dbtable=""trade"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""begtime"" msdata:Caption=""Beg time"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""begtime"" msprop:trade_x003A_columnformat=""GeneralDate"" msprop:expression="""" msprop:dbtable=""trade"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""endtime"" msdata:Caption=""End time"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""endtime"" msprop:trade_x003A_columnformat=""GeneralDate"" msprop:expression="""" msprop:dbtable=""trade"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""timezone"" msdata:Caption=""TZ"" msprop:dbcolumn=""timezone"" msprop:expression="""" msprop:dbtable=""trade"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""4""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""ferctransactionclass"" msdata:Caption=""FERC Transaction class"" msprop:dbcolumn=""ferctransactionclass"" msprop:expression="""" msprop:dbtable=""trade"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""24""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""broker"" msdata:Caption=""Broker"" msprop:dbcolumn=""broker"" msprop:expression="""" msprop:dbtable=""trade"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""sourcetrade"" msdata:Caption=""Source trade"" msprop:dbcolumn=""sourcetrade"" msprop:expression="""" msprop:dbtable=""trade"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""tradestatus"" msdata:Caption=""Trade status"" msprop:dbcolumn=""tradestatus"" msprop:expression="""" msprop:dbtable=""trade"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""contractprep"" msdata:Caption=""Contract prep"" msprop:dbcolumn=""contractprep"" msprop:expression="""" msprop:dbtable=""trade"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""timeseries"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""timeseries"" msprop:expression="""" msprop:proxycolumn=""timeseries"" msprop:dbtable=""position"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""status"" msdata:Caption=""Status"" msprop:dbcolumn=""status"" msprop:expression="""" msprop:dbtable=""trade"" default=""ACTIVE"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""confirmstatus"" msdata:Caption=""Confirm status"" msprop:dbcolumn=""confirmstatus"" msprop:expression="""" msprop:viewdefault=""NEW"" msprop:dbtable=""trade"" default=""NEW"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""tradeclass"" msdata:Caption=""Trade class"" msprop:dbcolumn=""tradeclass"" msprop:expression="""" msprop:dbtable=""trade"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""counterpartytrade"" msdata:Caption=""Counterparty trade"" msprop:dbcolumn=""counterpartytrade"" msprop:expression="""" msprop:dbtable=""trade"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""trader"" msdata:Caption=""Trader"" msprop:dbcolumn=""trader"" msprop:expression="""" msprop:viewdefault=""Julio Trujillo"" msprop:dbtable=""trade"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""counterpartytrader"" msdata:Caption=""Counterparty trader"" msprop:dbcolumn=""counterpartytrader"" msprop:expression="""" msprop:dbtable=""trade"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""brokertrader"" msdata:Caption=""Broker trader"" msprop:dbcolumn=""brokertrader"" msprop:expression="""" msprop:dbtable=""trade"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""settlementfreq"" msdata:Caption=""Settlement freq"" msprop:dbcolumn=""settlementfreq"" msprop:expression="""" msprop:dbtable=""trade"" default=""MONTH"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""currency"" msdata:Caption=""Currency"" msprop:dbcolumn=""currency"" msprop:expression="""" msprop:viewdefault=""USD"" msprop:dbtable=""trade"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""taxable"" msdata:Caption=""Taxable"" msprop:dbcolumn=""taxable"" msprop:expression="""" msprop:dbtable=""trade"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""evergreen"" msdata:Caption=""Evergreen"" msprop:dbcolumn=""evergreen"" msprop:expression="""" msprop:dbtable=""trade"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""evergreennotice"" msdata:Caption=""Evergreen notice"" msprop:dbcolumn=""evergreennotice"" msprop:expression="""" msprop:dbtable=""trade"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""evergreenrenewal"" msdata:Caption=""Evergreen renewal"" msprop:dbcolumn=""evergreenrenewal"" msprop:expression="""" msprop:dbtable=""trade"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""internal"" msdata:Caption=""Internal"" msprop:dbtable=""trade"" msprop:Generator_ColumnPropNameInTable=""internalColumn"" msprop:Generator_ColumnVarNameInTable=""columninternal"" msprop:expression="""" msprop:Generator_UserColumnName=""internal"" msprop:dbcolumn=""internal"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""internaltrade"" msdata:Caption=""Internal Trade"" msprop:dbcolumn=""internaltrade"" msprop:expression="""" msprop:dbtable=""trade"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""begdate"" msdata:Caption=""Beg date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""begdate"" msprop:trade_x003A_columnformat=""ShortDate"" msprop:expression="""" msprop:dbtable=""trade"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""enddate"" msdata:Caption=""End date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""enddate"" msprop:trade_x003A_columnformat=""ShortDate"" msprop:expression="""" msprop:dbtable=""trade"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""physicalexchange"" msdata:Caption=""Physical exchange"" msprop:dbcolumn=""physicalexchange"" msprop:expression="""" msprop:dbtable=""trade"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""holidaycalendar"" msdata:Caption=""Holiday calendar"" msprop:dbcolumn=""holidaycalendar"" msprop:expression="""" msprop:dbtable=""trade"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""titletransfer"" msdata:Caption=""Title transfer"" msprop:dbcolumn=""titletransfer"" msprop:expression="""" msprop:dbtable=""trade"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""executetrade"" msdata:Caption=""Execute"" msprop:dbcolumn=""executetrade"" msprop:expression="""" msprop:viewdefault=""1"" msprop:dbtable=""trade"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""evergreenforecast"" msdata:Caption=""Evergreen forecast"" msprop:dbcolumn=""evergreenforecast"" msprop:expression="""" msprop:dbtable=""trade"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""fallback"" msdata:Caption=""Fallback"" msprop:dbcolumn=""fallback"" msprop:expression="""" msprop:viewdefault=""0"" msprop:dbtable=""trade"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""electronicconfirmstatus"" msdata:Caption=""Electronic confirm status"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""document"" msdata:Caption=""Document"" msprop:dbcolumn=""document"" msprop:expression="""" msprop:dbtable=""trade"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""calendar"" msdata:Caption=""Calendar"" msprop:dbcolumn=""calendar"" msprop:expression="""" msprop:dbtable=""trade"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""correspondence"" msdata:Caption=""Correspondence"" msprop:dbcolumn=""correspondence"" msprop:expression="""" msprop:dbtable=""trade"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""remark"" msdata:Caption=""Remark"" msprop:dbcolumn=""remark"" msprop:expression="""" msprop:dbtable=""trade"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""collaboration"" msdata:Caption=""Collaboration"" msprop:dbcolumn=""collaboration"" msprop:expression="""" msprop:dbtable=""trade"" msprop:tempsysgen=""0"" default="""" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""creationname"" msdata:Caption=""Creation name"" msprop:dbcolumn=""creationname"" msprop:expression="""" msprop:dbtable=""trade"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""creationdate"" msdata:Caption=""Creation date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""creationdate"" msprop:expression="""" msprop:dbtable=""trade"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""revisionname"" msdata:Caption=""Revision name"" msprop:dbcolumn=""revisionname"" msprop:expression="""" msprop:dbtable=""trade"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""revisiondate"" msdata:Caption=""Revision date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""revisiondate"" msprop:expression="""" msprop:dbtable=""trade"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""product"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""product"" msprop:expression="""" msprop:proxycolumn=""product"" msprop:dbtable=""position"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""positionmode"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""positionmode"" msprop:expression="""" msprop:proxycolumn=""positionmode"" msprop:dbtable=""position"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""h_version"" msdata:Caption=""Version"" type=""xs:long"" default=""1"" minOccurs=""0""/>
											<xs:element name=""positiontype"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""positiontype"" msprop:expression="""" msprop:proxycolumn=""positiontype"" msprop:dbtable=""position"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""exchange"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""exchange"" msprop:expression="""" msprop:proxycolumn=""exchange"" msprop:dbtable=""position"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""symbol"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""symbol"" msprop:expression="""" msprop:proxycolumn=""symbol"" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""contractperiod"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""contractperiod"" msprop:expression="""" msprop:proxycolumn=""contractperiod"" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""marketarea"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""marketarea"" msprop:expression="""" msprop:proxycolumn=""marketarea"" msprop:dbtable=""position"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""companyeic"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""companyeic"" msprop:expression="""" msprop:proxycolumn=""companyeic"" msprop:dbtable=""powerposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""counterparty"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""counterparty"" msprop:expression="""" msprop:proxycolumn=""counterparty"" msprop:dbtable=""position"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""contract"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""contract"" msprop:expression="""" msprop:proxycolumn=""contract"" msprop:dbtable=""position"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""broker_contract"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""contract"" msprop:expression="""" msprop:proxycolumn=""contract"" msprop:dbtable=""position"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""tradebook"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""tradebook"" msprop:expression="""" msprop:proxycolumn=""tradebook"" msprop:dbtable=""position"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""quantity"" msprop:dbtable="""" msprop:proxypane=""tradedetail"" msprop:expression="""" msprop:proxycolumn=""quantity"" msprop:dbcolumn=""quantity"" msprop:trade_x003A_columnformat=""#,##0.00"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""counterpartyeic"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""counterpartyeic"" msprop:expression="""" msprop:proxycolumn=""counterpartyeic"" msprop:dbtable=""powerposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""unit"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""unit"" msprop:expression="""" msprop:proxycolumn=""unit"" msprop:dbtable=""position"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""blocktotal"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""blocktotal"" msprop:expression="""" msprop:proxycolumn=""blocktotal"" msprop:dbtable=""emissionquantity"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""timeunit"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""timeunit"" msprop:expression="""" msprop:proxycolumn=""timeunit"" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""fee_priceindex"" msprop:proxypane=""pricedetail"" msprop:dbcolumn=""priceindex"" msprop:expression="""" msprop:proxycolumn=""priceindex"" msprop:dbtable=""fee"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""fee_pricediff"" msprop:dbtable=""fee"" msprop:proxycolumn=""pricediff"" msprop:proxypane=""pricedetail"" msprop:viewdefault=""0"" msprop:expression="""" msprop:dbcolumn=""pricediff"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""feetimeperiod_timeperiod"" msprop:proxypane=""pricedetail"" msprop:dbcolumn=""timeperiod"" msprop:expression="""" msprop:proxycolumn=""timeperiod"" msprop:dbtable=""feetimeperiod"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""program"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""program"" msprop:expression="""" msprop:proxycolumn=""program"" msprop:dbtable=""emissionposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""fee_feemode"" msprop:proxypane=""pricedetail"" msprop:dbcolumn=""feemode"" msprop:expression="""" msprop:proxycolumn=""feemode"" msprop:dbtable=""fee"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""phase"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""phase"" msprop:expression="""" msprop:proxycolumn=""phase"" msprop:dbtable=""emissionposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""fee_unit"" msprop:proxypane=""pricedetail"" msprop:dbcolumn=""unit"" msprop:expression="""" msprop:proxycolumn=""unit"" msprop:dbtable=""fee"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""vintageyear"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""vintageyear"" msprop:expression="""" msprop:proxycolumn=""vintageyear"" msprop:dbtable=""emissionposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""fee_pricelevel"" msprop:dbtable=""fee"" msprop:proxycolumn=""pricelevel"" msprop:proxypane=""pricedetail"" msprop:viewdefault=""AVG"" msprop:expression="""" msprop:dbcolumn=""pricelevel"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""startcertificatenumber"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""startcertificatenumber"" msprop:expression="""" msprop:proxycolumn=""startcertificatenumber"" msprop:dbtable=""emissionquantity"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""efpfin_begtime"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn="""" msprop:trade_x003A_columnformat=""ShortDate"" msprop:expression="""" msprop:dbtable="""" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""optionstatus"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""optionstatus"" msprop:expression="""" msprop:proxycolumn=""optionstatus"" msprop:dbtable=""emissionquantity"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""efpfin_endtime"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn="""" msprop:trade_x003A_columnformat=""ShortDate"" msprop:expression="""" msprop:dbtable="""" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""efpfin_positiontype"" msprop:dbcolumn="""" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""efpfin_pricediff"" msprop:dbcolumn="""" msprop:trade_x003A_columnformat=""#,##0.00"" msprop:expression="""" msprop:dbtable="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""endcertificatenumber"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""endcertificatenumber"" msprop:expression="""" msprop:proxycolumn=""endcertificatenumber"" msprop:dbtable=""emissionquantity"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""efpfin_postdate"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn="""" msprop:trade_x003A_columnformat=""ShortDate"" msprop:expression="""" msprop:dbtable="""" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""efpfin_postprice"" msprop:dbcolumn="""" msprop:trade_x003A_columnformat=""#,##0.00"" msprop:expression="""" msprop:dbtable="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""efpfin_priceindex"" msprop:dbcolumn="""" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""rectype"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""rectype"" msprop:expression="""" msprop:proxycolumn=""rectype"" msprop:dbtable=""emissionposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""efpfin_contractquantity"" msprop:dbcolumn="""" msprop:trade_x003A_columnformat=""#,##0"" msprop:expression="""" msprop:dbtable="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""efpfin_quantity"" msprop:dbcolumn="""" msprop:trade_x003A_columnformat=""#,##0.00"" msprop:expression="""" msprop:dbtable="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""efpfin_timeunit"" msprop:dbcolumn="""" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""source"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""source"" msprop:expression="""" msprop:proxycolumn=""source"" msprop:dbtable=""emissionquantity"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""efpfin_unit"" msprop:dbcolumn="""" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""efpphys_begtime"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn="""" msprop:trade_x003A_columnformat=""ShortDate"" msprop:expression="""" msprop:dbtable="""" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""genunit"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""genunit"" msprop:expression="""" msprop:proxycolumn=""genunit"" msprop:dbtable=""emissionquantity"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""efpphys_endtime"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn="""" msprop:trade_x003A_columnformat=""ShortDate"" msprop:expression="""" msprop:dbtable="""" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""registry"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""registry"" msprop:expression="""" msprop:proxycolumn=""registry"" msprop:dbtable=""emissionquantity"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""efpphys_positiontype"" msprop:dbcolumn="""" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""efpphys_pricediff"" msprop:dbcolumn="""" msprop:trade_x003A_columnformat=""#,##0.00"" msprop:expression="""" msprop:dbtable="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""pricetype"" msdata:Caption=""Price type"" msprop:dbcolumn=""pricetype"" msprop:expression="""" msprop:viewdefault=""FIXED"" msprop:dbtable=""trade"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""efpphys_priceindex"" msprop:dbcolumn="""" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""paymentterms"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""paymentterms"" msprop:expression="""" msprop:proxycolumn=""paymentterms"" msprop:dbtable=""position"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""efpphys_quantity"" msprop:dbcolumn="""" msprop:trade_x003A_columnformat=""#,##0.00"" msprop:expression="""" msprop:dbtable="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""registryconfirmation"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""registryconfirmation"" msprop:expression="""" msprop:proxycolumn=""registryconfirmation"" msprop:dbtable=""emissionquantity"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""accountname"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""accountname"" msprop:expression="""" msprop:proxycolumn=""accountname"" msprop:dbtable=""emissionquantity"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""efpphys_timeunit"" msprop:dbcolumn="""" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""efpphys_carrier"" msprop:dbcolumn="""" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""efpphys_cycle"" msprop:dbcolumn="""" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""efpphys_delmethod"" msprop:dbcolumn="""" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""efpphys_incoterms"" msprop:dbcolumn="""" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""efpphys_location"" msprop:dbcolumn="""" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""efpphys_paymentterms"" msprop:dbcolumn="""" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""efpphys_spec"" msprop:dbcolumn="""" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""efpphys_unit"" msprop:dbcolumn="""" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""accttype"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""accttype"" msprop:expression="""" msprop:proxycolumn=""accttype"" msprop:dbtable=""emissionquantity"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""accountnumber"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""accountnumber"" msprop:expression="""" msprop:proxycolumn=""accountnumber"" msprop:dbtable=""emissionquantity"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""duedate"" msdata:DateTimeMode=""Unspecified"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""duedate"" msprop:expression="""" msprop:proxycolumn=""duedate"" msprop:dbtable=""emissionquantity"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""priority"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""priority"" msprop:expression="""" msprop:proxycolumn=""priority"" msprop:dbtable=""ngposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""pipelinecontract"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""pipelinecontract"" msprop:expression="""" msprop:proxycolumn=""pipelinecontract"" msprop:dbtable=""ngposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""ng_service"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""service"" msprop:expression="""" msprop:proxycolumn=""service"" msprop:dbtable=""ngposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""servicestatus"" msprop:dbtable=""ngposition"" msprop:proxycolumn=""servicestatus"" msprop:proxypane=""tradedetail"" msprop:viewdefault=""PRIMARY"" msprop:expression="""" msprop:dbcolumn=""servicestatus"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""segment"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""segment"" msprop:expression="""" msprop:proxycolumn=""segment"" msprop:dbtable=""ngposition"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""secondary"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""secondary"" msprop:expression="""" msprop:proxycolumn=""secondary"" msprop:dbtable=""ngposition"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""pipeline"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""pipeline"" msprop:expression="""" msprop:proxycolumn=""pipeline"" msprop:dbtable=""ngposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""ng_point"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""point"" msprop:expression="""" msprop:proxycolumn=""point"" msprop:dbtable=""ngposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""ng_recpoint"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""recpoint"" msprop:expression="""" msprop:proxycolumn=""recpoint"" msprop:dbtable=""ngposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""ng_delpoint"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""delpoint"" msprop:expression="""" msprop:proxycolumn=""delpoint"" msprop:dbtable=""ngposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""ng_property"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""property"" msprop:expression="""" msprop:proxycolumn=""property"" msprop:dbtable=""ngposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""shipperaccount"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""shipperaccount"" msprop:expression="""" msprop:proxycolumn=""shipperaccount"" msprop:dbtable=""ngposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""streamcontract"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""streamcontract"" msprop:expression="""" msprop:proxycolumn=""streamcontract"" msprop:dbtable=""ngposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""streamrank"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""streamrank"" msprop:expression="""" msprop:proxycolumn=""streamrank"" msprop:dbtable=""ngposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""firm"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""firm"" msprop:expression="""" msprop:proxycolumn=""firm"" msprop:dbtable=""ngposition"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""certificatetype"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""certificatetype"" msprop:expression="""" msprop:proxycolumn=""certificatetype"" msprop:dbtable=""emissionposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""ng_decint"" msprop:dbtable=""ngposition"" msprop:proxycolumn=""decint"" msprop:proxypane=""tradedetail"" msprop:viewdefault=""1"" msprop:expression="""" msprop:dbcolumn=""decint"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""ng_feecode"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""feecode"" msprop:expression="""" msprop:proxycolumn=""feecode"" msprop:dbtable=""ngposition"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""quantitymethod"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""quantitymethod"" msprop:expression="""" msprop:proxycolumn=""quantitymethod"" msprop:dbtable=""ngposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""ng_evergreentermdate"" msdata:DateTimeMode=""Unspecified"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""evergreentermdate"" msprop:expression="""" msprop:proxycolumn=""evergreentermdate"" msprop:dbtable=""ngposition"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""phys_product"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""product"" msprop:expression="""" msprop:proxycolumn=""product"" msprop:dbtable=""position"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""location"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""location"" msprop:expression="""" msprop:proxycolumn=""location"" msprop:dbtable=""physicalposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""origin"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""origin"" msprop:expression="""" msprop:proxycolumn=""origin"" msprop:dbtable=""physicalposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""destination"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""destination"" msprop:expression="""" msprop:proxycolumn=""destination"" msprop:dbtable=""physicalposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""tank"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""tank"" msprop:expression="""" msprop:proxycolumn=""tank"" msprop:dbtable=""physicalposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""pile"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""pile"" msprop:expression="""" msprop:proxycolumn=""pile"" msprop:dbtable=""physicalposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""phys_property"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""property"" msprop:expression="""" msprop:proxycolumn=""property"" msprop:dbtable=""physicalposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""power_decint"" msprop:dbtable=""powerposition"" msprop:proxycolumn=""decint"" msprop:proxypane=""tradedetail"" msprop:viewdefault=""1"" msprop:expression="""" msprop:dbcolumn=""decint"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""carrier"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""carrier"" msprop:expression="""" msprop:proxycolumn=""carrier"" msprop:dbtable=""physicalposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""delmethod"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""delmethod"" msprop:expression="""" msprop:proxycolumn=""delmethod"" msprop:dbtable=""physicalposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""cycle"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""cycle"" msprop:expression="""" msprop:proxycolumn=""cycle"" msprop:dbtable=""physicalquantity"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""incoterms"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""incoterms"" msprop:expression="""" msprop:proxycolumn=""incoterms"" msprop:dbtable=""physicalposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""custodychain"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""custodychain"" msprop:expression="""" msprop:proxycolumn=""custodychain"" msprop:dbtable=""physicalposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""forwardquantity"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""forwardquantity"" msprop:expression="""" msprop:proxycolumn=""forwardquantity"" msprop:dbtable=""physicalposition"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""origshipment"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""origshipment"" msprop:expression="""" msprop:proxycolumn=""origshipment"" msprop:dbtable=""physicalquantity"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""phys_decint"" msprop:dbtable=""physicalposition"" msprop:proxycolumn=""decint"" msprop:proxypane=""tradedetail"" msprop:viewdefault=""1"" msprop:expression="""" msprop:dbcolumn=""decint"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""phys_evergreentermdate"" msdata:DateTimeMode=""Unspecified"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""evergreentermdate"" msprop:expression="""" msprop:proxycolumn=""evergreentermdate"" msprop:dbtable=""physicalposition"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""locationbasis"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""locationbasis"" msprop:expression="""" msprop:proxycolumn=""locationbasis"" msprop:dbtable=""physicalposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""efpfin_product"" msprop:dbcolumn="""" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""productbasis"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""productbasis"" msprop:expression="""" msprop:proxycolumn=""productbasis"" msprop:dbtable=""physicalposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""controlarea"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""controlarea"" msprop:expression="""" msprop:proxycolumn=""controlarea"" msprop:dbtable=""powerposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""efpphys_product"" msprop:dbcolumn="""" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""power_point"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""point"" msprop:expression="""" msprop:proxycolumn=""point"" msprop:dbtable=""powerposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""power_recpoint"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""recpoint"" msprop:expression="""" msprop:proxycolumn=""recpoint"" msprop:dbtable=""powerposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""power_delpoint"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""delpoint"" msprop:expression="""" msprop:proxycolumn=""delpoint"" msprop:dbtable=""powerposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""tsstage"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""tsstage"" msprop:expression="""" msprop:proxycolumn=""tsstage"" msprop:dbtable=""powerposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""rank"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""rank"" msprop:expression="""" msprop:proxycolumn=""rank"" msprop:dbtable=""powerposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""oasis"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""oasis"" msprop:expression="""" msprop:proxycolumn=""oasis"" msprop:dbtable=""powerposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""tsnumber"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""tsnumber"" msprop:expression="""" msprop:proxycolumn=""tsnumber"" msprop:dbtable=""powerposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""tsnumberref"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""tsnumberref"" msprop:expression="""" msprop:proxycolumn=""tsnumberref"" msprop:dbtable=""powerposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""power_service"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""service"" msprop:expression="""" msprop:proxycolumn=""service"" msprop:dbtable=""powerposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""transtatus"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""transtatus"" msprop:expression="""" msprop:proxycolumn=""transtatus"" msprop:dbtable=""powerposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""tsclass"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""tsclass"" msprop:expression="""" msprop:proxycolumn=""tsclass"" msprop:dbtable=""powerposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""tsincrement"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""tsincrement"" msprop:expression="""" msprop:proxycolumn=""tsincrement"" msprop:dbtable=""powerposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""tstype"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""tstype"" msprop:expression="""" msprop:proxycolumn=""tstype"" msprop:dbtable=""powerposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""losspct"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""losspct"" msprop:expression="""" msprop:proxycolumn=""losspct"" msprop:dbtable=""powerposition"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""paycongestion"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""paycongestion"" msprop:expression="""" msprop:proxycolumn=""paycongestion"" msprop:dbtable=""powerposition"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""preconfirmed"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""preconfirmed"" msprop:expression="""" msprop:proxycolumn=""preconfirmed"" msprop:dbtable=""powerposition"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""generationunit"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""generationunit"" msprop:expression="""" msprop:proxycolumn=""generationunit"" msprop:dbtable=""powerposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""gencontrolarea"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""gencontrolarea"" msprop:expression="""" msprop:proxycolumn=""gencontrolarea"" msprop:dbtable=""powerposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""genpoint"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""genpoint"" msprop:expression="""" msprop:proxycolumn=""genpoint"" msprop:dbtable=""powerposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""genpct"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""genpct"" msprop:expression="""" msprop:proxycolumn=""genpct"" msprop:dbtable=""powerposition"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""power_feecode"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""feecode"" msprop:expression="""" msprop:proxycolumn=""feecode"" msprop:dbtable=""powerposition"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""power_iso"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""iso"" msprop:expression="""" msprop:proxycolumn=""iso"" msprop:dbtable=""powerposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""power_bidtype"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""bidtype"" msprop:expression="""" msprop:proxycolumn=""bidtype"" msprop:dbtable=""powerposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""power_markettype"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""markettype"" msprop:expression="""" msprop:proxycolumn=""markettype"" msprop:dbtable=""powerposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""power_participant"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""participant"" msprop:expression="""" msprop:proxycolumn=""participant"" msprop:dbtable=""powerposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""contractquantity"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""contractquantity"" msprop:expression="""" msprop:proxycolumn=""contractquantity"" msprop:dbtable=""finposition"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""constraintmethod"" msprop:proxypane=""positionconstraint"" msprop:dbcolumn=""constraintmethod"" msprop:expression="""" msprop:proxycolumn=""constraintmethod"" msprop:dbtable=""positionconstraint"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""constrainttype"" msprop:proxypane=""positionconstraint"" msprop:dbcolumn=""constrainttype"" msprop:expression="""" msprop:proxycolumn=""constrainttype"" msprop:dbtable=""positionconstraint"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""maximum"" msprop:proxypane=""positionconstraint"" msprop:dbcolumn=""maximum"" msprop:expression="""" msprop:proxycolumn=""maximum"" msprop:dbtable=""positionconstraint"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""feetimeperiod_event"" msprop:proxypane=""pricedetail"" msprop:dbcolumn=""priceevent"" msprop:expression="""" msprop:proxycolumn=""priceevent"" msprop:dbtable=""feetimeperiod"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""minimum"" msprop:proxypane=""positionconstraint"" msprop:dbcolumn=""minimum"" msprop:expression="""" msprop:proxycolumn=""minimum"" msprop:dbtable=""positionconstraint"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""exchbroker"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""exchbroker"" msprop:expression="""" msprop:proxycolumn=""exchbroker"" msprop:dbtable=""finposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""constraint_unit"" msprop:proxypane=""positionconstraint"" msprop:dbcolumn=""unit"" msprop:expression="""" msprop:proxycolumn=""unit"" msprop:dbtable=""positionconstraint"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""exchbrokercontract"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""exchbrokercontract"" msprop:expression="""" msprop:proxycolumn=""exchbrokercontract"" msprop:dbtable=""finposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""brokeraccount"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""brokeraccount"" msprop:expression="""" msprop:proxycolumn=""brokeraccount"" msprop:dbtable=""finposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""postdate"" msdata:DateTimeMode=""Unspecified"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""postdate"" msprop:expression="""" msprop:proxycolumn=""postdate"" msprop:dbtable=""finposition"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""valuedate"" msdata:DateTimeMode=""Unspecified"" msprop:dbtable=""finposition"" msprop:proxypane=""tradedetail"" msprop:expression="""" msprop:proxycolumn=""valuedate"" msprop:dbcolumn=""valuedate"" msprop:trade_x003A_columnformat=""ShortDate"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""maturitydate"" msdata:DateTimeMode=""Unspecified"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""maturitydate"" msprop:expression="""" msprop:proxycolumn=""maturitydate"" msprop:dbtable=""finposition"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""rate"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""rate"" msprop:expression="""" msprop:proxycolumn=""rate"" msprop:dbtable=""finposition"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""forwardpoints"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""forwardpoints"" msprop:expression="""" msprop:proxycolumn=""forwardpoints"" msprop:dbtable=""finposition"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""fixdate"" msdata:DateTimeMode=""Unspecified"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""fixdate"" msprop:expression="""" msprop:proxycolumn=""fixdate"" msprop:dbtable=""finposition"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""fee_currency"" msprop:dbtable=""fee"" msprop:proxycolumn=""currency"" msprop:proxypane=""pricedetail"" msprop:viewdefault=""USD"" msprop:expression="""" msprop:dbcolumn=""currency"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""fee_paystatus"" msprop:proxypane=""pricedetail"" msprop:dbcolumn=""paystatus"" msprop:expression="""" msprop:proxycolumn=""paystatus"" msprop:dbtable=""fee"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""block"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""block"" msprop:expression="""" msprop:proxycolumn=""block"" msprop:dbtable=""position"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""optiontype"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""optiontype"" msprop:expression="""" msprop:proxycolumn=""optiontype"" msprop:dbtable=""position"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""optionstyle"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""optionstyle"" msprop:expression="""" msprop:proxycolumn=""optionstyle"" msprop:dbtable=""position"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""loadshape"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""loadshape"" msprop:expression="""" msprop:proxycolumn=""loadshape"" msprop:dbtable=""position"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""strikeprice"" msprop:dbtable=""position"" msprop:proxycolumn=""strikeprice"" msprop:proxypane=""tradedetail"" msprop:viewdefault=""0"" msprop:expression="""" msprop:dbcolumn=""strikeprice"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""premium"" msprop:dbtable=""position"" msprop:proxycolumn=""premium"" msprop:proxypane=""tradedetail"" msprop:viewdefault=""0"" msprop:expression="""" msprop:dbcolumn=""premium"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""premiumduedate"" msdata:DateTimeMode=""Unspecified"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""premiumduedate"" msprop:expression="""" msprop:proxycolumn=""premiumduedate"" msprop:dbtable=""position"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""settlementdate"" msdata:DateTimeMode=""Unspecified"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""settlementdate"" msprop:expression="""" msprop:proxycolumn=""settlementdate"" msprop:dbtable=""position"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""premiummethod"" msprop:dbtable=""position"" msprop:proxycolumn=""premiummethod"" msprop:proxypane=""tradedetail"" msprop:viewdefault=""VARIABLE"" msprop:expression="""" msprop:dbcolumn=""premiummethod"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""settlementcurrency"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""settlementcurrency"" msprop:expression="""" msprop:proxycolumn=""settlementcurrency"" msprop:dbtable=""position"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""feetimeperiod_begtime"" msdata:DateTimeMode=""Unspecified"" msprop:proxypane=""pricedetail"" msprop:dbcolumn=""begtime"" msprop:expression="""" msprop:proxycolumn=""begtime"" msprop:dbtable=""feetimeperiod"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""optionfrequency"" msprop:dbtable=""position"" msprop:proxycolumn=""optionfrequency"" msprop:proxypane=""tradedetail"" msprop:viewdefault=""MONTH"" msprop:expression="""" msprop:dbcolumn=""optionfrequency"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""feetimeperiod_endtime"" msdata:DateTimeMode=""Unspecified"" msprop:proxypane=""pricedetail"" msprop:dbcolumn=""endtime"" msprop:expression="""" msprop:proxycolumn=""endtime"" msprop:dbtable=""feetimeperiod"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""expirationdate"" msdata:DateTimeMode=""Unspecified"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""expirationdate"" msprop:expression="""" msprop:proxycolumn=""expirationdate"" msprop:dbtable=""position"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""feetimeperiod_daytype"" msprop:proxypane=""pricedetail"" msprop:dbcolumn=""timeperioddaytype"" msprop:expression="""" msprop:proxycolumn=""timeperioddaytype"" msprop:dbtable=""feetimeperiod"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""optionposition"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""optionposition"" msprop:expression="""" msprop:proxycolumn=""optionposition"" msprop:dbtable=""position"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""settlementunit"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""settlementunit"" msprop:expression="""" msprop:proxycolumn=""settlementunit"" msprop:dbtable=""position"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""production"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""production"" msprop:expression="""" msprop:proxycolumn=""production"" msprop:dbtable=""position"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""consumption"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""consumption"" msprop:expression="""" msprop:proxycolumn=""consumption"" msprop:dbtable=""position"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""pay_currency"" msprop:dbtable=""fee"" msprop:proxycolumn=""currency"" msprop:proxypane=""pricedetail"" msprop:viewdefault=""USD"" msprop:expression="""" msprop:dbcolumn=""currency"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""pay_pricediff"" msprop:dbtable=""fee"" msprop:proxycolumn=""pricediff"" msprop:proxypane=""pricedetail"" msprop:viewdefault=""0"" msprop:expression="""" msprop:dbcolumn=""pricediff"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""pay_priceindex"" msprop:proxypane=""pricedetail"" msprop:dbcolumn=""priceindex"" msprop:expression="""" msprop:proxycolumn=""priceindex"" msprop:dbtable=""fee"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""pay_feemode"" msprop:proxypane=""pricedetail"" msprop:dbcolumn=""feemode"" msprop:expression="""" msprop:proxycolumn=""feemode"" msprop:dbtable=""fee"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""pay_unit"" msprop:proxypane=""pricedetail"" msprop:dbcolumn=""unit"" msprop:expression="""" msprop:proxycolumn=""unit"" msprop:dbtable=""fee"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""rec_currency"" msprop:dbtable=""fee"" msprop:proxycolumn=""currency"" msprop:proxypane=""pricedetail"" msprop:viewdefault=""USD"" msprop:expression="""" msprop:dbcolumn=""currency"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""rec_pricediff"" msprop:dbtable=""fee"" msprop:proxycolumn=""pricediff"" msprop:proxypane=""pricedetail"" msprop:viewdefault=""0"" msprop:expression="""" msprop:dbcolumn=""pricediff"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""rec_priceindex"" msprop:proxypane=""pricedetail"" msprop:dbcolumn=""priceindex"" msprop:expression="""" msprop:proxycolumn=""priceindex"" msprop:dbtable=""fee"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""rec_feemode"" msprop:proxypane=""pricedetail"" msprop:dbcolumn=""feemode"" msprop:expression="""" msprop:proxycolumn=""feemode"" msprop:dbtable=""fee"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""rec_unit"" msprop:proxypane=""pricedetail"" msprop:dbcolumn=""unit"" msprop:expression="""" msprop:proxycolumn=""unit"" msprop:dbtable=""fee"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""strike_currency"" msprop:dbtable=""fee"" msprop:proxycolumn=""currency"" msprop:proxypane=""pricedetail"" msprop:viewdefault=""USD"" msprop:expression="""" msprop:dbcolumn=""currency"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""strike_feemode"" msprop:proxypane=""pricedetail"" msprop:dbcolumn=""feemode"" msprop:expression="""" msprop:proxycolumn=""feemode"" msprop:dbtable=""fee"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""strike_pricediff"" msprop:dbtable=""fee"" msprop:proxycolumn=""pricediff"" msprop:proxypane=""pricedetail"" msprop:viewdefault=""0"" msprop:expression="""" msprop:dbcolumn=""pricediff"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""strike_priceindex"" msprop:proxypane=""pricedetail"" msprop:dbcolumn=""priceindex"" msprop:expression="""" msprop:proxycolumn=""priceindex"" msprop:dbtable=""fee"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""strike_unit"" msprop:proxypane=""pricedetail"" msprop:dbcolumn=""unit"" msprop:expression="""" msprop:proxycolumn=""unit"" msprop:dbtable=""fee"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""reference_currency"" msprop:dbtable=""fee"" msprop:proxycolumn=""currency"" msprop:proxypane=""pricedetail"" msprop:viewdefault=""USD"" msprop:expression="""" msprop:dbcolumn=""currency"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""reference_pricediff"" msprop:dbtable=""fee"" msprop:proxycolumn=""pricediff"" msprop:proxypane=""pricedetail"" msprop:viewdefault=""0"" msprop:expression="""" msprop:dbcolumn=""pricediff"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""reference_priceindex"" msprop:proxypane=""pricedetail"" msprop:dbcolumn=""priceindex"" msprop:expression="""" msprop:proxycolumn=""priceindex"" msprop:dbtable=""fee"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""reference_feemode"" msprop:proxypane=""pricedetail"" msprop:dbcolumn=""feemode"" msprop:expression="""" msprop:proxycolumn=""feemode"" msprop:dbtable=""fee"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""reference_unit"" msprop:proxypane=""pricedetail"" msprop:dbcolumn=""unit"" msprop:expression="""" msprop:proxycolumn=""unit"" msprop:dbtable=""fee"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""cascadereference"" msdata:Caption=""Cascade reference"" msprop:dbcolumn=""cascadereference"" msprop:expression="""" msprop:dbtable=""trade"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""swaptype"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""swaptype"" msprop:expression="""" msprop:proxycolumn=""swaptype"" msprop:dbtable=""position"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""heatrate"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""factor"" msprop:expression="""" msprop:proxycolumn=""heatrate"" msprop:dbtable=""fee"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""constraint_timeunit"" msprop:proxypane=""positionconstraint"" msprop:dbcolumn=""timeunit"" msprop:expression="""" msprop:proxycolumn=""timeunit"" msprop:dbtable=""positionconstraint"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""decint"" msprop:dbtable=""powerposition"" msprop:proxycolumn=""decint"" msprop:proxypane=""tradedetail"" msprop:viewdefault=""1"" msprop:expression="""" msprop:dbcolumn=""decint"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""ldcmeter"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""ldcmeter"" msprop:expression="""" msprop:proxycolumn=""ldcmeter"" msprop:dbtable=""ngposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""efp_positiontype"" msprop:dbcolumn="""" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""fee_fixingdate"" msdata:DateTimeMode=""Unspecified"" msprop:dbtable=""fee"" msprop:proxypane=""pricedetail"" msprop:expression="""" msprop:proxycolumn=""fixingdate"" msprop:dbcolumn=""fixingdate"" msprop:trade_x003A_columnformat=""ShortDateTime"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""trade_h_version"" msdata:Caption=""Version"" msprop:dbcolumn=""h_version"" msprop:expression="""" msprop:dbtable=""trade"" type=""xs:int"" default=""1"" minOccurs=""0""/>
											<xs:element name=""position_h_version"" msprop:proxypane=""tradedetail"" msprop:dbcolumn=""h_version"" msprop:expression="""" msprop:proxycolumn=""position_h_version"" msprop:dbtable=""position"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""RowNumber"" type=""xs:int"" minOccurs=""0""/>
										</xs:sequence>
									</xs:complexType>
								</xs:element>
								<xs:element name=""positionquality"" msprop:ProxyVersion=""v2"" msprop:viewpane=""positionquality"">
									<xs:complexType>
										<xs:sequence>
											<xs:element name=""fee"" msdata:Caption=""Fee"" msprop:dbcolumn=""fee"" msprop:expression="""" msprop:dbtable=""positionquality"" msprop:tempsysgen=""0"" default="""">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""position"" msdata:Caption=""Position"" msprop:dbcolumn=""position"" msprop:expression="""" msprop:dbtable=""positionquality"" msprop:tempsysgen=""0"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""product"" msdata:Caption=""Product"" msprop:dbcolumn=""product"" msprop:expression="""" msprop:dbtable=""positionquality"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""24""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""quality"" msdata:Caption=""Quality"" msprop:dbcolumn=""quality"" msprop:expression="""" msprop:dbtable=""positionquality"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""standardvalue"" msdata:Caption=""Standard value"" msprop:dbcolumn=""standardvalue"" msprop:positionquality_x003A_columnformat=""#,##0.000000"" msprop:expression="""" msprop:dbtable=""positionquality"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""minvalue"" msdata:Caption=""Min value"" msprop:dbcolumn=""minvalue"" msprop:positionquality_x003A_columnformat=""#,##0.000000"" msprop:expression="""" msprop:dbtable=""positionquality"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""maxvalue"" msdata:Caption=""Max value"" msprop:dbcolumn=""maxvalue"" msprop:positionquality_x003A_columnformat=""#,##0.000000"" msprop:expression="""" msprop:dbtable=""positionquality"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""cvdifferential"" msdata:Caption=""CV differential"" msprop:dbcolumn=""cvdifferential"" msprop:positionquality_x003A_columnformat=""#,##0"" msprop:expression="""" msprop:dbtable=""positionquality"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""deemprice"" msdata:Caption=""Deem price"" msprop:dbcolumn=""deemprice"" msprop:positionquality_x003A_columnformat=""#,##0.00000000"" msprop:expression="""" msprop:dbtable=""positionquality"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""factorquality"" msdata:Caption=""Factor quality"" msprop:dbcolumn=""factorquality"" msprop:expression="""" msprop:dbtable=""positionquality"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""rejectvalue"" msdata:Caption=""Reject value"" msprop:dbcolumn=""rejectvalue"" msprop:positionquality_x003A_columnformat=""#,##0.000000"" msprop:expression="""" msprop:dbtable=""positionquality"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""adjustment"" msdata:Caption=""Adjustment"" msprop:dbcolumn=""adjustment"" msprop:positionquality_x003A_columnformat=""#,##0.000000"" msprop:expression="""" msprop:dbtable=""positionquality"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""method"" msdata:Caption=""Method"" msprop:dbcolumn=""method"" msprop:expression="""" msprop:viewdefault=""SHIPMENT"" msprop:dbtable=""positionquality"" default=""SHIPMENT"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""qualitymethod"" msdata:Caption=""Quality method"" msprop:dbcolumn=""qualitymethod"" msprop:expression="""" msprop:dbtable=""positionquality"" default=""PERCENT"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""incrvalue"" msdata:Caption=""Incr value"" msprop:dbcolumn=""incrvalue"" msprop:positionquality_x003A_columnformat=""#,##0.000000"" msprop:expression="""" msprop:dbtable=""positionquality"" type=""xs:decimal"" default=""1"" minOccurs=""0""/>
											<xs:element name=""currency"" msdata:Caption=""Currency"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""premium"" msdata:Caption=""Premium"" msprop:dbcolumn=""premium"" msprop:positionquality_x003A_columnformat=""Boolean"" msprop:expression="""" msprop:dbtable=""positionquality"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""penalty"" msdata:Caption=""Penalty"" msprop:dbcolumn=""penalty"" msprop:positionquality_x003A_columnformat=""Boolean"" msprop:expression="""" msprop:dbtable=""positionquality"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""priceindex"" msdata:Caption=""Price index"" msprop:dbcolumn=""priceindex"" msprop:expression="""" msprop:dbtable=""positionquality"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""qualitybasis"" msdata:Caption=""Quality Basis"" msprop:dbcolumn=""qualitybasis"" msprop:expression="""" msprop:dbtable=""positionquality"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""factor"" msdata:Caption=""Factor"" msprop:dbcolumn=""factor"" msprop:expression="""" msprop:dbtable=""positionquality"" type=""xs:decimal"" default=""1"" minOccurs=""0""/>
											<xs:element name=""qualitystandard"" msdata:Caption=""Quality Standard"" msprop:dbcolumn=""qualitystandard"" msprop:expression="""" msprop:dbtable=""positionquality"" default=""ASTM"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""priceprecision"" msdata:Caption=""Price precision"" type=""xs:decimal"" default=""5"" minOccurs=""0""/>
											<xs:element name=""settlequalitygroup"" msdata:Caption=""Settle quality group"" msprop:dbcolumn=""settlequalitygroup"" msprop:expression="""" msprop:dbtable=""positionquality"" default=""SHIPMENT"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""settlequalityperiod"" msdata:Caption=""Settle quality period"" msprop:dbcolumn=""settlequalityperiod"" msprop:expression="""" msprop:dbtable=""positionquality"" default=""MONTHLY"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""minrejectvalue"" msdata:Caption=""Min reject value"" msprop:dbcolumn=""minrejectvalue"" msprop:positionquality_x003A_columnformat=""#,##0.000000"" msprop:expression="""" msprop:dbtable=""positionquality"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""maxrejectvalue"" msdata:Caption=""Max reject value"" msprop:dbcolumn=""maxrejectvalue"" msprop:positionquality_x003A_columnformat=""#,##0.000000"" msprop:expression="""" msprop:dbtable=""positionquality"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""creationname"" msdata:Caption=""Creation name"" msprop:dbcolumn=""creationname"" msprop:expression="""" msprop:dbtable=""positionquality"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""creationdate"" msdata:Caption=""Creation date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""creationdate"" msprop:positionquality_x003A_columnformat=""ShortDateTime"" msprop:expression="""" msprop:dbtable=""positionquality"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""revisionname"" msdata:Caption=""Revision name"" msprop:dbcolumn=""revisionname"" msprop:expression="""" msprop:dbtable=""positionquality"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""revisiondate"" msdata:Caption=""Revision date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""revisiondate"" msprop:positionquality_x003A_columnformat=""ShortDateTime"" msprop:expression="""" msprop:dbtable=""positionquality"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""RowNumber"" type=""xs:int"" minOccurs=""0""/>
										</xs:sequence>
									</xs:complexType>
								</xs:element>
								<xs:element name=""positionconstraint"" msprop:ProxyVersion=""v2"" msprop:viewpane=""positionconstraint"">
									<xs:complexType>
										<xs:sequence>
											<xs:element name=""position"" msdata:Caption=""Position"" msprop:dbcolumn=""position"" msprop:expression="""" msprop:dbtable=""positionconstraint"" default="""">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""surrogate"" msdata:AutoIncrement=""true"" msdata:AutoIncrementStep=""-1"" msdata:Caption=""Surrogate"" msprop:dbcolumn=""surrogate"" msprop:expression="""" msprop:dbtable=""positionconstraint"" msprop:tempsysgen=""0"" type=""xs:decimal""/>
											<xs:element name=""constrainttype"" msdata:Caption=""Type"" msprop:dbcolumn=""constrainttype"" msprop:expression="""" msprop:dbtable=""positionconstraint"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""minimum"" msdata:Caption=""Minimum"" msprop:dbcolumn=""minimum"" msprop:expression="""" msprop:dbtable=""positionconstraint"" msprop:positionconstraint_x003A_columnformat=""#,##0.00"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""maximum"" msdata:Caption=""Maximum"" msprop:dbcolumn=""maximum"" msprop:expression="""" msprop:dbtable=""positionconstraint"" msprop:positionconstraint_x003A_columnformat=""#,##0.00"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""unit"" msdata:Caption=""Unit"" msprop:dbcolumn=""unit"" msprop:expression="""" msprop:dbtable=""positionconstraint"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""timeunit"" msdata:Caption=""Time  unit"" msprop:dbcolumn=""timeunit"" msprop:expression="""" msprop:dbtable=""positionconstraint"" default=""DAY"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""constraintmethod"" msdata:Caption=""Method"" msprop:dbcolumn=""constraintmethod"" msprop:expression="""" msprop:dbtable=""positionconstraint"" default=""ABSOLUTE"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""creationname"" msdata:Caption=""Creation name"" msprop:dbcolumn=""creationname"" msprop:expression="""" msprop:dbtable=""positionconstraint"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""creationdate"" msdata:Caption=""Creation date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""creationdate"" msprop:expression="""" msprop:dbtable=""positionconstraint"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""revisionname"" msdata:Caption=""Revision name"" msprop:dbcolumn=""revisionname"" msprop:expression="""" msprop:dbtable=""positionconstraint"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""revisiondate"" msdata:Caption=""Revision date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""revisiondate"" msprop:expression="""" msprop:dbtable=""positionconstraint"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""serviceproduct"" msdata:Caption=""Service product"" msprop:dbcolumn=""serviceproduct"" msprop:expression="""" msprop:dbtable=""positionconstraint"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""24""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""RowNumber"" type=""xs:int"" minOccurs=""0""/>
										</xs:sequence>
									</xs:complexType>
								</xs:element>
								<xs:element name=""tradeforwardcurve"" msprop:ProxyVersion=""v2"" msprop:viewpane=""tradeforwardcurve"">
									<xs:complexType>
										<xs:sequence>
											<xs:element name=""position"" msdata:Caption=""Position"" msprop:dbcolumn=""position"" msprop:expression="""" msprop:dbtable=""tradeforwardcurve"" default="""">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""trade"" msdata:Caption=""Trade"" msprop:dbcolumn=""trade"" msprop:expression="""" msprop:dbtable=""tradeforwardcurve"" default="""">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""surrogate"" msdata:AutoIncrement=""true"" msdata:AutoIncrementStep=""-1"" msdata:Caption=""Surrogate"" type=""xs:decimal""/>
											<xs:element name=""timeperiod"" msdata:Caption=""Time period"" msprop:dbcolumn=""timeperiod"" msprop:expression="""" msprop:dbtable=""tradeforwardcurve"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""timeperiodbegtime"" msdata:Caption=""TP Beg time"" msdata:DateTimeMode=""Unspecified"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""timeperiodendtime"" msdata:Caption=""TP End time"" msdata:DateTimeMode=""Unspecified"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""timeperioddaytype"" msdata:Caption=""TP Day type"" default=""CALENDAR"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""begtime"" msdata:Caption=""Beg time"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""begtime"" msprop:expression="""" msprop:tradeforwardcurve_x003A_columnformat=""ShortDateTime"" msprop:dbtable=""tradeforwardcurve"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""endtime"" msdata:Caption=""End time"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""endtime"" msprop:expression="""" msprop:tradeforwardcurve_x003A_columnformat=""ShortDateTime"" msprop:dbtable=""tradeforwardcurve"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""priceindex"" msdata:Caption=""Price index"" msprop:dbcolumn=""priceindex"" msprop:expression="""" msprop:dbtable=""tradeforwardcurve"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""pricelevel"" msdata:Caption=""Price level"" msprop:dbcolumn=""pricelevel"" msprop:expression="""" msprop:dbtable=""tradeforwardcurve"" default=""AVG"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""pricediff"" msdata:Caption=""Price diff"" msprop:dbcolumn=""pricediff"" msprop:expression="""" msprop:tradeforwardcurve_x003A_columnformat=""#,##0.000000"" msprop:dbtable=""tradeforwardcurve"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""promptoffsetdays"" msdata:Caption=""Prompt offset days"" msprop:dbcolumn=""promptoffsetdays"" msprop:expression="""" msprop:tradeforwardcurve_x003A_columnformat=""#,##0"" msprop:dbtable=""tradeforwardcurve"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""marketperiod"" msdata:Caption=""Market period"" msprop:dbcolumn=""marketperiod"" msprop:expression="""" msprop:dbtable=""tradeforwardcurve"" default=""DELIVERY"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""timemethod"" msdata:Caption=""Time method"" msprop:dbcolumn=""timemethod"" msprop:expression="""" msprop:dbtable=""tradeforwardcurve"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""priceevent"" msdata:Caption=""Price event"" msprop:dbcolumn=""priceevent"" msprop:expression="""" msprop:dbtable=""tradeforwardcurve"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""eventdate"" msdata:Caption=""Event date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""eventdate"" msprop:expression="""" msprop:tradeforwardcurve_x003A_columnformat=""ShortDate"" msprop:dbtable=""tradeforwardcurve"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""creationname"" msdata:Caption=""Creation name"" msprop:dbcolumn=""creationname"" msprop:expression="""" msprop:dbtable=""tradeforwardcurve"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""creationdate"" msdata:Caption=""Creation date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""creationdate"" msprop:expression="""" msprop:tradeforwardcurve_x003A_columnformat=""ShortDateTime"" msprop:dbtable=""tradeforwardcurve"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""revisionname"" msdata:Caption=""Revision name"" msprop:dbcolumn=""revisionname"" msprop:expression="""" msprop:dbtable=""tradeforwardcurve"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""revisiondate"" msdata:Caption=""Revision date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""revisiondate"" msprop:expression="""" msprop:tradeforwardcurve_x003A_columnformat=""ShortDateTime"" msprop:dbtable=""tradeforwardcurve"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""RowNumber"" type=""xs:int"" minOccurs=""0""/>
										</xs:sequence>
									</xs:complexType>
								</xs:element>
								<xs:element name=""tradeexercise"" msprop:ProxyVersion=""v2"" msprop:viewpane=""tradeexercise"">
									<xs:complexType>
										<xs:sequence>
											<xs:element name=""position"" msdata:Caption=""Position"" msprop:dbcolumn=""position"" msprop:expression="""" msprop:dbtable=""tradeexercise"" default="""">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""surrogate"" msdata:AutoIncrement=""true"" msdata:AutoIncrementStep=""-1"" msdata:Caption=""Surrogate"" type=""xs:decimal""/>
											<xs:element name=""trade"" msdata:Caption=""Trade"" msprop:dbcolumn=""trade"" msprop:expression="""" msprop:dbtable=""tradeexercise"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""begtime"" msdata:Caption=""Beg time"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""begtime"" msprop:expression="""" msprop:tradeexercise_x003A_columnformat=""ShortDateTime"" msprop:dbtable=""tradeexercise"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""endtime"" msdata:Caption=""End time"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""endtime"" msprop:expression="""" msprop:tradeexercise_x003A_columnformat=""ShortDateTime"" msprop:dbtable=""tradeexercise"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""exerciseposition"" msdata:Caption=""Exercise position"" msprop:dbcolumn=""exerciseposition"" msprop:expression="""" msprop:dbtable=""tradeexercise"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""optionstatus"" msdata:Caption=""Option status"" msprop:dbcolumn=""optionstatus"" msprop:expression="""" msprop:dbtable=""tradeexercise"" default=""EXERCISE"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""exercisedate"" msdata:Caption=""Exercise date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""exercisedate"" msprop:expression="""" msprop:tradeexercise_x003A_columnformat=""ShortDateTime"" msprop:dbtable=""tradeexercise"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""exercisetrade"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""quantity"" msdata:Caption=""Quantity"" msprop:dbcolumn=""quantity"" msprop:tradeexercise_x003A_subtotal=""True"" msprop:expression="""" msprop:tradeexercise_x003A_columnformat=""#,##0.0000"" msprop:dbtable=""tradeexercise"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""price"" msdata:Caption=""Price"" msprop:dbcolumn=""price"" msprop:expression="""" msprop:tradeexercise_x003A_columnformat=""#,##0.000000"" msprop:dbtable=""tradeexercise"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""terminationdate"" msdata:Caption=""Termination date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""terminationdate"" msprop:expression="""" msprop:tradeexercise_x003A_columnformat=""ShortDateTime"" msprop:dbtable=""tradeexercise"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""indexprice"" msdata:Caption=""Index price"" msprop:dbcolumn=""indexprice"" msprop:expression="""" msprop:tradeexercise_x003A_columnformat=""#,##0.00000"" msprop:dbtable=""tradeexercise"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""unit"" msdata:Caption=""Unit"" msprop:dbcolumn=""unit"" msprop:expression="""" msprop:dbtable=""tradeexercise"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""priceunit"" msdata:Caption=""Price unit"" msprop:dbcolumn=""priceunit"" msprop:expression="""" msprop:dbtable=""tradeexercise"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""currency"" msdata:Caption=""Currency"" msprop:dbcolumn=""currency"" msprop:expression="""" msprop:dbtable=""tradeexercise"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""creationname"" msdata:Caption=""Creation name"" msprop:dbcolumn=""creationname"" msprop:expression="""" msprop:dbtable=""tradeexercise"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""creationdate"" msdata:Caption=""Creation date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""creationdate"" msprop:expression="""" msprop:tradeexercise_x003A_columnformat=""ShortDateTime"" msprop:dbtable=""tradeexercise"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""revisionname"" msdata:Caption=""Revision name"" msprop:dbcolumn=""revisionname"" msprop:expression="""" msprop:dbtable=""tradeexercise"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""revisiondate"" msdata:Caption=""Revision date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""revisiondate"" msprop:expression="""" msprop:tradeexercise_x003A_columnformat=""ShortDateTime"" msprop:dbtable=""tradeexercise"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""exerciseposseq"" msdata:Caption=""Exercise pos seq"" msprop:dbcolumn=""exerciseposseq"" msprop:expression="""" msprop:dbtable=""tradeexercise"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""RowNumber"" type=""xs:int"" minOccurs=""0""/>
										</xs:sequence>
									</xs:complexType>
								</xs:element>
								<xs:element name=""valuationdetail"" msprop:ProxyVersion=""v2"" msprop:viewpane=""valuationdetail"">
									<xs:complexType>
										<xs:sequence>
											<xs:element name=""valuation"" msdata:Caption=""Valuation"" msprop:dbcolumn=""valuation"" msprop:expression="""" msprop:dbtable=""valuationdetail"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""surrogate"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""valuationdetail"" msdata:Caption=""Valuationdetail"" type=""xs:decimal"" default=""0""/>
											<xs:element name=""valuationmode"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""position"" msdata:Caption=""Position"" msprop:dbcolumn=""position"" msprop:expression="""" msprop:dbtable=""valuationdetail"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""valuationperiod"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""valuationexposure"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""valuationproduct"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""quantitystatus"" msdata:Caption=""Quantity status"" msprop:dbcolumn=""quantitystatus"" msprop:expression="""" msprop:dbtable=""valuationdetail"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""positionstatus"" msdata:Caption=""Position status"" msprop:dbcolumn=""positionstatus"" msprop:expression="""" msprop:dbtable=""valuationdetail"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""settlementstatus"" msdata:Caption=""Settlement status"" msprop:dbcolumn=""settlementstatus"" msprop:expression="""" msprop:dbtable=""valuationdetail"" msprop:valuationdetail_x003A_columnformat=""Boolean"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""evergreenstatus"" msdata:Caption=""Evergreen status"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""pricedate"" msdata:Caption=""Price date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""pricedate"" msprop:expression="""" msprop:dbtable=""valuationdetail"" msprop:valuationdetail_x003A_columnformat=""ShortDateTime"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""quantity"" msdata:Caption=""Quantity"" msprop:dbcolumn=""quantity"" msprop:expression="""" msprop:dbtable=""valuationdetail"" msprop:valuationdetail_x003A_columnformat=""#,##0.000000"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""pricequantity"" msdata:Caption=""Price quantity"" msprop:dbcolumn=""pricequantity"" msprop:expression="""" msprop:dbtable=""valuationdetail"" msprop:valuationdetail_x003A_columnformat=""#,##0.000000"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""exposurequantity"" msdata:Caption=""Exposure quantity"" msprop:dbcolumn=""exposurequantity"" msprop:expression="""" msprop:dbtable=""valuationdetail"" msprop:valuationdetail_x003A_columnformat=""#,##0.000000"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""unit"" msdata:Caption=""Unit"" msprop:dbcolumn=""unit"" msprop:expression="""" msprop:dbtable=""valuationdetail"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""value"" msdata:Caption=""Value"" msprop:dbcolumn=""value"" msprop:expression="""" msprop:dbtable=""valuationdetail"" msprop:valuationdetail_x003A_columnformat=""#,##0.000000"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""price"" msdata:Caption=""Price"" msprop:dbcolumn=""price"" msprop:expression="""" msprop:dbtable=""valuationdetail"" msprop:valuationdetail_x003A_columnformat=""#,##0.000000"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""marketvalue"" msdata:Caption=""Market value"" msprop:dbcolumn=""marketvalue"" msprop:expression="""" msprop:dbtable=""valuationdetail"" msprop:valuationdetail_x003A_columnformat=""#,##0.000000"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""marketprice"" msdata:Caption=""Market Price"" msprop:dbcolumn=""marketprice"" msprop:expression="""" msprop:dbtable=""valuationdetail"" msprop:valuationdetail_x003A_columnformat=""#,##0.000000"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""currency"" msdata:Caption=""Currency"" msprop:dbcolumn=""currency"" msprop:expression="""" msprop:dbtable=""valuationdetail"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""priceunit"" msdata:Caption=""Price unit"" msprop:dbcolumn=""priceunit"" msprop:expression="""" msprop:dbtable=""valuationdetail"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""npvfactor"" msdata:Caption=""NPV factor"" msprop:dbcolumn=""npvfactor"" msprop:expression="""" msprop:dbtable=""valuationdetail"" msprop:valuationdetail_x003A_columnformat=""#,##0.00000000"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""validation"" msdata:Caption=""Validation"" msprop:dbcolumn=""validation"" msprop:expression="""" msprop:dbtable=""valuationdetail"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""1024""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""strikeprice"" msdata:Caption=""Strike price"" msprop:dbcolumn=""strikeprice"" msprop:expression="""" msprop:dbtable=""valuationdetail"" msprop:valuationdetail_x003A_columnformat=""#,##0.000000"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""optrefprice"" msdata:Caption=""Option reference price"" msprop:dbcolumn=""optrefprice"" msprop:expression="""" msprop:dbtable=""valuationdetail"" msprop:valuationdetail_x003A_columnformat=""#,##0.000000"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""intrate"" msdata:Caption=""Interest rate"" msprop:dbcolumn=""intrate"" msprop:expression="""" msprop:dbtable=""valuationdetail"" msprop:valuationdetail_x003A_columnformat=""#,##0.000000"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""optionvalue"" msdata:Caption=""Option value"" msprop:dbcolumn=""optionvalue"" msprop:expression="""" msprop:dbtable=""valuationdetail"" msprop:valuationdetail_x003A_columnformat=""#,##0.000000"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""delta"" msdata:Caption=""Delta"" msprop:dbcolumn=""delta"" msprop:expression="""" msprop:dbtable=""valuationdetail"" msprop:valuationdetail_x003A_columnformat=""#,##0.000000"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""gamma"" msdata:Caption=""Gamma"" msprop:dbcolumn=""gamma"" msprop:expression="""" msprop:dbtable=""valuationdetail"" msprop:valuationdetail_x003A_columnformat=""#,##0.000000"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""theta"" msdata:Caption=""Theta"" msprop:dbcolumn=""theta"" msprop:expression="""" msprop:dbtable=""valuationdetail"" msprop:valuationdetail_x003A_columnformat=""#,##0.000000"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""vega"" msdata:Caption=""Vega"" msprop:dbcolumn=""vega"" msprop:expression="""" msprop:dbtable=""valuationdetail"" msprop:valuationdetail_x003A_columnformat=""#,##0.000000"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""rho"" msdata:Caption=""Rho"" msprop:dbcolumn=""rho"" msprop:expression="""" msprop:dbtable=""valuationdetail"" msprop:valuationdetail_x003A_columnformat=""#,##0.000000"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""volatility"" msdata:Caption=""Volatility"" msprop:dbcolumn=""volatility"" msprop:expression="""" msprop:dbtable=""valuationdetail"" msprop:valuationdetail_x003A_columnformat=""#,##0.000000"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""promptvolatility"" msdata:Caption=""Prompt Volatility"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""var"" msdata:Caption=""VaR"" msprop:dbcolumn=""var"" msprop:expression="""" msprop:dbtable=""valuationdetail"" msprop:valuationdetail_x003A_columnformat=""#,##0.00"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""creditexposure"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""creditvar"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""findetail"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""creationname"" msdata:Caption=""Creation name"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""creationdate"" msdata:Caption=""Creation date"" msdata:DateTimeMode=""Unspecified"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""revisionname"" msdata:Caption=""Revision name"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""revisiondate"" msdata:Caption=""Revision date"" msdata:DateTimeMode=""Unspecified"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""transactiontype"" msdata:Caption=""Transaction type"" default=""JE"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""4""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""trade"" msdata:Caption=""Trade"" msprop:dbcolumn=""trade"" msprop:expression="""" msprop:dbtable=""valuationdetail"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""exposure"" msdata:Caption=""Exposure"" msprop:dbcolumn=""exposure"" msprop:expression="""" msprop:dbtable=""valuationdetail"" default=""POSITION"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""posdetail"" msdata:Caption=""Pos Detail"" msprop:dbcolumn=""posdetail"" msprop:expression="""" msprop:dbtable=""valuationdetail"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""fee"" msdata:Caption=""Fee"" msprop:dbcolumn=""fee"" msprop:expression="""" msprop:dbtable=""valuationdetail"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""tier"" msdata:Caption=""Tier"" msprop:dbcolumn=""tier"" msprop:expression="""" msprop:dbtable=""valuationdetail"" msprop:valuationdetail_x003A_columnformat=""#,##0"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""begtime"" msdata:Caption=""Beg time"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""begtime"" msprop:expression="""" msprop:dbtable=""valuationdetail"" msprop:valuationdetail_x003A_columnformat=""ShortDateTime"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""endtime"" msdata:Caption=""End time"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""endtime"" msprop:expression="""" msprop:dbtable=""valuationdetail"" msprop:valuationdetail_x003A_columnformat=""ShortDateTime"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""mtmvalue"" msprop:dbcolumn=""mtmvalue"" msprop:expression="""" msprop:valuationdetail_x003A_subtotal=""True"" msprop:dbtable="""" msprop:valuationdetail_x003A_columnformat=""#,##0"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""pricestatus"" msdata:Caption=""Price status"" msprop:dbcolumn=""pricestatus"" msprop:expression="""" msprop:dbtable=""valuationdetail"" default=""FLOAT"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""description"" msdata:Caption=""Description"" msprop:dbcolumn=""description"" msprop:expression="""" msprop:dbtable=""valuationdetail"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""1024""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""cashnpvfactor"" msdata:Caption=""Cash npv factor"" msprop:dbcolumn=""cashnpvfactor"" msprop:expression="""" msprop:dbtable=""valuationdetail"" msprop:valuationdetail_x003A_columnformat=""#,##0.00000000"" type=""xs:decimal"" default=""1"" minOccurs=""0""/>
											<xs:element name=""expectedduedate"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn="""" msprop:expression="""" msprop:dbtable=""valuationdetail"" msprop:valuationdetail_x003A_columnformat=""ShortDateTime"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""positiontype"" msprop:dbcolumn="""" msprop:expression="""" msprop:viewdefault=""&lt;null&gt;"" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""currencyfactor"" msdata:Caption=""Currency factor"" msprop:dbcolumn=""currencyfactor"" msprop:expression="""" msprop:dbtable=""valuationdetail"" msprop:valuationdetail_x003A_columnformat=""#,##0"" type=""xs:decimal"" default=""1"" minOccurs=""0""/>
											<xs:element name=""expirationdate"" msdata:Caption=""Expiration Date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""expirationdate"" msprop:expression="""" msprop:dbtable=""valuationdetail"" msprop:valuationdetail_x003A_columnformat=""ShortDate"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""shipment"" msdata:Caption=""Shipment"" msprop:dbcolumn=""shipment"" msprop:expression="""" msprop:dbtable=""valuationdetail"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""strategy"" msdata:Caption=""Strategy"" msprop:dbcolumn=""strategy"" msprop:expression="""" msprop:dbtable=""valuationdetail"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""product"" msdata:Caption=""Product"" msprop:dbcolumn=""product"" msprop:expression="""" msprop:dbtable=""valuationdetail"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""24""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""priceindex"" msdata:Caption=""Price index"" msprop:dbcolumn=""priceindex"" msprop:expression="""" msprop:dbtable=""valuationdetail"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""timezone"" msdata:Caption=""TZ"" msprop:dbcolumn=""timezone"" msprop:expression="""" msprop:dbtable=""valuationdetail"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""4""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""tradebook"" msdata:Caption=""Trade book"" msprop:dbcolumn=""tradebook"" msprop:expression="""" msprop:dbtable=""valuationdetail"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""trader"" msdata:Caption=""Trader"" msprop:dbcolumn=""trader"" msprop:expression="""" msprop:dbtable=""valuationdetail"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""tsperiod"" msdata:Caption=""TS period"" msprop:dbcolumn=""tsperiod"" msprop:expression="""" msprop:dbtable=""valuationdetail"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""exposuretype"" msdata:Caption=""Exposure type"" msprop:dbcolumn=""exposuretype"" msprop:expression="""" msprop:dbtable=""valuationdetail"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""hedge"" msdata:Caption=""Hedge"" msprop:dbcolumn=""hedge"" msprop:expression="""" msprop:dbtable=""valuationdetail"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""location"" msdata:Caption=""Location"" msprop:dbcolumn=""location"" msprop:expression="""" msprop:dbtable=""valuationdetail"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""marketarea"" msdata:Caption=""Market area"" msprop:dbcolumn=""marketarea"" msprop:expression="""" msprop:dbtable=""valuationdetail"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""feetype"" msdata:Caption=""Fee type"" msprop:dbcolumn=""feetype"" msprop:expression="""" msprop:dbtable=""valuationdetail"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""futuremonth"" msdata:Caption=""Future month"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""futuremonth"" msprop:expression="""" msprop:dbtable=""valuationdetail"" msprop:valuationdetail_x003A_columnformat=""ShortDate"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""measure"" msdata:Caption=""Measure"" msprop:dbcolumn=""measure"" msprop:expression="""" msprop:dbtable=""valuationdetail"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""block"" msdata:Caption=""Block"" msprop:dbcolumn=""block"" msprop:expression="""" msprop:dbtable=""valuationdetail"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""carrier"" msdata:Caption=""Carrier"" msprop:dbcolumn=""carrier"" msprop:expression="""" msprop:dbtable=""valuationdetail"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""company"" msdata:Caption=""Company"" msprop:dbcolumn=""company"" msprop:expression="""" msprop:dbtable=""valuationdetail"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""component"" msdata:Caption=""Component"" msprop:dbcolumn=""component"" msprop:expression="""" msprop:dbtable=""valuationdetail"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""24""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""compositeindex"" msdata:Caption=""Composite index"" msprop:dbcolumn=""compositeindex"" msprop:expression="""" msprop:dbtable=""valuationdetail"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""contract"" msdata:Caption=""Contract"" msprop:dbcolumn=""contract"" msprop:expression="""" msprop:dbtable=""valuationdetail"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""counterparty"" msdata:Caption=""Counterparty"" msprop:dbcolumn=""counterparty"" msprop:expression="""" msprop:dbtable=""valuationdetail"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""settlementdate"" msdata:Caption=""Settlement date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""settlementdate"" msprop:expression="""" msprop:dbtable=""valuationdetail"" msprop:valuationdetail_x003A_columnformat=""ShortDate"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""optiontype"" msdata:Caption=""Option type"" msprop:dbcolumn=""optiontype"" msprop:expression="""" msprop:dbtable=""position"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""exposuremonth"" msdata:Caption=""Exposure month"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""exposuremonth"" msprop:expression="""" msprop:dbtable=""valuationdetail"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""exposureindex"" msdata:Caption=""Exposure index"" msprop:dbcolumn=""exposureindex"" msprop:expression="""" msprop:dbtable=""valuationdetail"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""fxbegtime"" msdata:Caption=""Fx begtime"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""fxbegtime"" msprop:expression="""" msprop:dbtable=""valuationdetail"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""fxendtime"" msdata:Caption=""Fx endtime"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""fxendtime"" msprop:expression="""" msprop:dbtable=""valuationdetail"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""fxcurrency"" msprop:dbcolumn=""fxcurrency"" msprop:expression="""" msprop:dbtable=""valuationdetail"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""fxattribution"" msdata:Caption=""Fx attribution"" msprop:dbcolumn=""fxattribution"" msprop:expression="""" msprop:dbtable=""valuationdetail"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""fxexposure"" msprop:dbcolumn=""fxexposure"" msprop:expression="""" msprop:dbtable=""valuationdetail"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""fxrisk"" msprop:dbcolumn=""fxrisk"" msprop:expression="""" msprop:dbtable=""valuationdetail"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""fxstatus"" msprop:dbcolumn=""fxstatus"" msprop:expression="""" msprop:dbtable=""valuationdetail"" default=""FIXED"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""fxvolatility"" msdata:Caption=""Fx volatility"" msprop:dbcolumn=""fxvolatility"" msprop:expression="""" msprop:dbtable=""valuationdetail"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""RowNumber"" type=""xs:int"" minOccurs=""0""/>
										</xs:sequence>
									</xs:complexType>
								</xs:element>
								<xs:element name=""summary"" msprop:ProxyVersion=""v2"" msprop:InRefreshingDrillTables=""False"" msprop:viewpane=""summary"">
									<xs:complexType>
										<xs:sequence>
											<xs:element name=""corporate"" msprop:dbcolumn=""corporate"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""creditparty"" msprop:dbtable="""" msprop:summary_x003A_drill=""True"" msprop:summary_x003A_sortseq=""1"" msprop:summary_x003A_sortorder=""ASC"" msprop:expression="""" msprop:dbcolumn=""creditparty"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""description"" msprop:dbcolumn=""description"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""limit"" msprop:summary_x003A_subtotal=""True"" msprop:dbcolumn=""limit"" msprop:summary_x003A_columnformat=""#,##0;(#,##0)"" msprop:expression="""" msprop:dbtable="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""collateral"" msprop:summary_x003A_subtotal=""True"" msprop:dbcolumn=""collateral"" msprop:summary_x003A_columnformat=""#,##0;(#,##0)"" msprop:expression="""" msprop:dbtable="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""openinvoice"" msprop:summary_x003A_subtotal=""True"" msprop:dbcolumn=""openinvoice"" msprop:summary_x003A_columnformat=""#,##0;(#,##0)"" msprop:expression="""" msprop:dbtable="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""current"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""forward"" msprop:summary_x003A_subtotal=""True"" msprop:dbcolumn=""forward"" msprop:summary_x003A_columnformat=""#,##0;(#,##0)"" msprop:expression="""" msprop:dbtable="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""exposure"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""contract"" msprop:dbtable="""" msprop:summary_x003A_drill=""True"" msprop:summary_x003A_sortseq=""2"" msprop:summary_x003A_sortorder=""ASC"" msprop:expression="""" msprop:dbcolumn=""contract"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""credittype"" msprop:dbtable="""" msprop:summary_x003A_drill=""True"" msprop:summary_x003A_sortseq=""3"" msprop:summary_x003A_sortorder=""ASC"" msprop:expression="""" msprop:dbcolumn=""credittype"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""currentexposure"" msprop:summary_x003A_subtotal=""True"" msprop:dbcolumn=""currentexposure"" msprop:summary_x003A_columnformat=""#,##0;(#,##0)"" msprop:expression="""" msprop:dbtable="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""totalexposure"" msprop:summary_x003A_subtotal=""True"" msprop:dbcolumn=""totalexposure"" msprop:summary_x003A_columnformat=""#,##0;(#,##0)"" msprop:expression="""" msprop:dbtable="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""currency"" msprop:dbcolumn=""currency"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""begtime"" msdata:DateTimeMode=""Unspecified"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""endtime"" msdata:DateTimeMode=""Unspecified"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""company"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""m0"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""groupdesc"" msprop:dbcolumn=""groupdesc"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""groupstatus"" type=""xs:int"" minOccurs=""0""/>
											<xs:element name=""grouplevel"" type=""xs:int"" minOccurs=""0""/>
											<xs:element name=""grouptotal"" type=""xs:int"" minOccurs=""0""/>
											<xs:element name=""counterparty"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""creditdetailreference"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""creditexposure"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""deliveredexposure"" msprop:summary_x003A_subtotal=""True"" msprop:dbcolumn="""" msprop:summary_x003A_columnformat=""#,##0;(#,##0)"" msprop:expression="""" msprop:dbtable="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""availablecredit"" msprop:summary_x003A_subtotal=""True"" msprop:dbcolumn="""" msprop:summary_x003A_columnformat=""#,##0;(#,##0)"" msprop:expression="""" msprop:dbtable="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""m"" msprop:summary_x003A_subtotal=""True"" msprop:dbcolumn=""m"" msprop:summary_x003A_columnformat=""#,##0;(#,##0)"" msprop:expression="""" msprop:dbtable="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""currencyfactor"" msdata:Caption=""Currency factor"" msprop:dbcolumn=""currencyfactor"" msprop:summary_x003A_columnformat=""#,##0"" msprop:expression="""" msprop:dbtable=""valuationdetail"" type=""xs:decimal"" default=""1"" minOccurs=""0""/>
											<xs:element name=""RowNumber"" type=""xs:int"" minOccurs=""0""/>
											<xs:element name=""grouplevel0_summary"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""grouplevel1_summary"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""grouplevel2_summary"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""m-0"" msdata:Caption=""Prior Not Invoiced"" msprop:summary_x003A_subtotal=""True"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""m-1"" msdata:Caption=""nov. 2024"" msprop:summary_x003A_subtotal=""True"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""m-2"" msdata:Caption=""dic. 2024"" msprop:summary_x003A_subtotal=""True"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""m-3"" msdata:Caption=""ene. 2025"" msprop:summary_x003A_subtotal=""True"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""m-4"" msdata:Caption=""feb. 2025"" msprop:summary_x003A_subtotal=""True"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""m-5"" msdata:Caption=""mar. 2025"" msprop:summary_x003A_subtotal=""True"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""m-6"" msdata:Caption=""abr. 2025"" msprop:summary_x003A_subtotal=""True"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""m-7"" msdata:Caption=""may. 2025"" msprop:summary_x003A_subtotal=""True"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""m-8"" msdata:Caption=""jun. 2025"" msprop:summary_x003A_subtotal=""True"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""m-9"" msdata:Caption=""jul. 2025"" msprop:summary_x003A_subtotal=""True"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""m-10"" msdata:Caption=""ago. 2025"" msprop:summary_x003A_subtotal=""True"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""m-11"" msdata:Caption=""sep. 2025"" msprop:summary_x003A_subtotal=""True"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""m-12"" msdata:Caption=""oct. 2025"" msprop:summary_x003A_subtotal=""True"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""m-13"" msdata:Caption=""nov. 2025"" msprop:summary_x003A_subtotal=""True"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""m-14"" msdata:Caption=""dic. 2025"" msprop:summary_x003A_subtotal=""True"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""m-15"" msdata:Caption=""ene. 2026"" msprop:summary_x003A_subtotal=""True"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""m-16"" msdata:Caption=""feb. 2026"" msprop:summary_x003A_subtotal=""True"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""m-17"" msdata:Caption=""mar. 2026"" msprop:summary_x003A_subtotal=""True"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""m-18"" msdata:Caption=""abr. 2026"" msprop:summary_x003A_subtotal=""True"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""m-19"" msdata:Caption=""may. 2026"" msprop:summary_x003A_subtotal=""True"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""m-20"" msdata:Caption=""jun. 2026"" msprop:summary_x003A_subtotal=""True"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""m-21"" msdata:Caption=""jul. 2026"" msprop:summary_x003A_subtotal=""True"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""m-22"" msdata:Caption=""ago. 2026"" msprop:summary_x003A_subtotal=""True"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""m-23"" msdata:Caption=""sep. 2026"" msprop:summary_x003A_subtotal=""True"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""m-24"" msdata:Caption=""oct. 2026"" msprop:summary_x003A_subtotal=""True"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""m-25"" msdata:Caption=""nov. 2026"" msprop:summary_x003A_subtotal=""True"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""m-26"" msdata:Caption=""dic. 2026"" msprop:summary_x003A_subtotal=""True"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""m-27"" msdata:Caption=""ene. 2027"" msprop:summary_x003A_subtotal=""True"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""m-28"" msdata:Caption=""feb. 2027"" msprop:summary_x003A_subtotal=""True"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""m-29"" msdata:Caption=""mar. 2027"" msprop:summary_x003A_subtotal=""True"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""m-30"" msdata:Caption=""abr. 2027"" msprop:summary_x003A_subtotal=""True"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""m-31"" msdata:Caption=""may. 2027"" msprop:summary_x003A_subtotal=""True"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""m-32"" msdata:Caption=""jun. 2027"" msprop:summary_x003A_subtotal=""True"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""m-33"" msdata:Caption=""jul. 2027"" msprop:summary_x003A_subtotal=""True"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""m-34"" msdata:Caption=""ago. 2027"" msprop:summary_x003A_subtotal=""True"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""m-35"" msdata:Caption=""sep. 2027"" msprop:summary_x003A_subtotal=""True"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""m-36"" msdata:Caption=""oct. 2027"" msprop:summary_x003A_subtotal=""True"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""m-37"" msdata:Caption=""nov. 2027"" msprop:summary_x003A_subtotal=""True"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""m-38"" msdata:Caption=""dic. 2027"" msprop:summary_x003A_subtotal=""True"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
										</xs:sequence>
									</xs:complexType>
								</xs:element>
								<xs:element name=""tradedetail"" msprop:InRefreshingDrillTables=""False"" msprop:ActiveRowIndex=""1"" msprop:panevisible=""True"" msprop:ProxyVersion=""v2"" msprop:viewpane=""tradedetail"">
									<xs:complexType>
										<xs:sequence>
											<xs:element name=""position"" msdata:Caption=""Position"" msprop:dbtable=""position"" msprop:tradedetail_x003A_sortseq=""1"" msprop:expression="""" msprop:dbcolumn=""position"" msprop:tempsysgen=""1"" msprop:tradedetail_x003A_sortorder=""ASC"" default="""">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""contract"" msdata:Caption=""Contract"" msprop:dbcolumn=""contract"" msprop:expression="""" msprop:dbtable=""position"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""trade"" msdata:Caption=""Trade"" msprop:dbcolumn=""trade"" msprop:expression="""" msprop:dbtable=""position"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""positionmode"" msdata:Caption=""Position mode"" msprop:dbcolumn=""positionmode"" msprop:expression="""" msprop:dbtable=""position"" default=""PHYSICAL"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""positiontype"" msdata:Caption=""Position type"" msprop:dbcolumn=""positiontype"" msprop:expression="""" msprop:dbtable=""position"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""timeseries"" msdata:Caption=""Time series"" msprop:dbcolumn=""timeseries"" msprop:expression="""" msprop:dbtable=""position"" msprop:tradedetail_x003A_columnformat=""Boolean"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""marketarea"" msdata:Caption=""Marketarea"" msprop:dbcolumn=""marketarea"" msprop:expression="""" msprop:dbtable=""position"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""counterparty"" msdata:Caption=""Counterparty"" msprop:dbcolumn=""counterparty"" msprop:expression="""" msprop:dbtable=""position"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""unit"" msdata:Caption=""Unit"" msprop:dbcolumn=""unit"" msprop:expression="""" msprop:dbtable=""position"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""company"" msdata:Caption=""Company"" msprop:dbcolumn=""company"" msprop:expression="""" msprop:viewdefault=""VISTA"" msprop:dbtable=""position"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""product"" msdata:Caption=""Product"" msprop:dbcolumn=""product"" msprop:expression="""" msprop:dbtable=""position"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""24""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""tradebook"" msdata:Caption=""Trade book"" msprop:dbcolumn=""tradebook"" msprop:expression="""" msprop:dbtable=""position"" default=""UNASSIGNED"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""block"" msdata:Caption=""Block"" msprop:dbcolumn=""block"" msprop:expression="""" msprop:dbtable=""position"" msprop:columnreq=""false"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""exchange"" msdata:Caption=""Exchange"" msprop:dbcolumn=""exchange"" msprop:expression="""" msprop:dbtable=""position"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""loadshape"" msdata:Caption=""Load shape"" msprop:dbcolumn=""loadshape"" msprop:expression="""" msprop:dbtable=""position"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""settlementdate"" msdata:Caption=""Settlement date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""settlementdate"" msprop:expression="""" msprop:dbtable=""position"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""paymentterms"" msdata:Caption=""Payment terms"" msprop:dbcolumn=""paymentterms"" msprop:expression="""" msprop:dbtable=""position"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""settlementcurrency"" msdata:Caption=""Currency"" msprop:dbcolumn=""settlementcurrency"" msprop:expression="""" msprop:dbtable=""position"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""optionposition"" msdata:Caption=""Option"" msprop:dbcolumn=""optionposition"" msprop:expression="""" msprop:dbtable=""position"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""optiontype"" msdata:Caption=""Option type"" msprop:dbcolumn=""optiontype"" msprop:expression="""" msprop:dbtable=""position"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""optionstyle"" msdata:Caption=""Option style"" msprop:dbcolumn=""optionstyle"" msprop:expression="""" msprop:dbtable=""position"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""premium"" msdata:Caption=""Premium"" msprop:dbcolumn=""premium"" msprop:expression="""" msprop:viewdefault=""0"" msprop:dbtable=""position"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""premiumduedate"" msdata:Caption=""Premium due date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""premiumduedate"" msprop:expression="""" msprop:dbtable=""position"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""premiummethod"" msdata:Caption=""Premium method"" msprop:dbcolumn=""premiummethod"" msprop:expression="""" msprop:viewdefault=""VARIABLE"" msprop:dbtable=""position"" default=""VARIABLE"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""optionfrequency"" msdata:Caption=""Option frequency"" msprop:dbcolumn=""optionfrequency"" msprop:expression="""" msprop:viewdefault=""MONTH"" msprop:dbtable=""position"" default=""MONTH"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""expirationdate"" msdata:Caption=""Expiration date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""expirationdate"" msprop:expression="""" msprop:dbtable=""position"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""settlementunit"" msdata:Caption=""Settlement unit"" msprop:dbcolumn=""settlementunit"" msprop:expression="""" msprop:dbtable=""position"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""settlementfrequency"" msdata:Caption=""Settlement Frequency"" msprop:dbcolumn=""settlementfrequency"" msprop:expression="""" msprop:dbtable=""position"" default=""MONTH"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""production"" msdata:Caption=""Production"" msprop:dbcolumn=""production"" msprop:expression="""" msprop:dbtable=""position"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""consumption"" msdata:Caption=""Consumption"" msprop:dbcolumn=""consumption"" msprop:expression="""" msprop:dbtable=""position"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""strikeprice"" msdata:Caption=""Strike price"" msprop:dbcolumn=""strikeprice"" msprop:expression="""" msprop:viewdefault=""0"" msprop:dbtable=""position"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""collaboration"" msdata:Caption=""Collaboration"" msprop:tempsysgen=""1"" msprop:dbtable=""position"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""internalposition"" msdata:Caption=""Internal Position"" msprop:dbcolumn=""internalposition"" msprop:expression="""" msprop:dbtable=""position"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""tradeexercise"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""positiondbtable"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""posdetail"" msprop:dbcolumn=""posdetail"" msprop:expression="""" msprop:dbtable="""" msprop:tempsysgen=""1"" type=""xs:string"" default=""""/>
											<xs:element name=""begtime"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""begtime"" msprop:expression="""" msprop:dbtable="""" msprop:tradedetail_x003A_columnformat=""ShortDateTime"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""endtime"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""endtime"" msprop:expression="""" msprop:dbtable="""" msprop:tradedetail_x003A_columnformat=""ShortDateTime"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""contractquantity"" msdata:Caption=""Contract quantity"" msprop:dbcolumn=""contractquantity"" msprop:expression="""" msprop:dbtable=""finposition"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""quantity"" msprop:dbcolumn="""" msprop:expression="""" msprop:dbtable="""" msprop:tradedetail_x003A_columnformat=""#,##0.00"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""timeunit"" msprop:dbcolumn=""timeunit"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""exchbroker"" msdata:Caption=""Exch broker"" msprop:dbcolumn=""exchbroker"" msprop:expression="""" msprop:dbtable=""finposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""exchbrokercontract"" msdata:Caption=""Exch broker contract"" msprop:dbcolumn=""exchbrokercontract"" msprop:expression="""" msprop:dbtable=""finposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""brokeraccount"" msdata:Caption=""Broker account"" msprop:dbcolumn=""brokeraccount"" msprop:expression="""" msprop:dbtable=""finposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""postdate"" msdata:Caption=""Post date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""postdate"" msprop:expression="""" msprop:dbtable=""finposition"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""valuedate"" msdata:Caption=""Value date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""valuedate"" msprop:expression="""" msprop:dbtable=""finposition"" type=""xs:dateTime"" default=""2024-11-18T00:00:00"" minOccurs=""0""/>
											<xs:element name=""maturitydate"" msdata:Caption=""Maturity date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""maturitydate"" msprop:expression="""" msprop:dbtable=""finposition"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""rate"" msdata:Caption=""Rate"" msprop:dbcolumn=""rate"" msprop:expression="""" msprop:dbtable=""finposition"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""forwardpoints"" msdata:Caption=""Forward points"" msprop:dbcolumn=""forwardpoints"" msprop:expression="""" msprop:dbtable=""finposition"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""fixdate"" msdata:Caption=""Fix date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""fixdate"" msprop:expression="""" msprop:dbtable=""finposition"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""feecode"" msprop:dbcolumn=""feecode"" msprop:expression="""" msprop:dbtable="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""pipeline"" msdata:Caption=""Pipeline"" msprop:dbcolumn=""pipeline"" msprop:expression="""" msprop:dbtable=""ngposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""point"" msprop:dbcolumn=""point"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""recpoint"" msprop:dbcolumn=""recpoint"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""delpoint"" msprop:dbcolumn=""delpoint"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""property"" msprop:dbcolumn=""property"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""shipperaccount"" msdata:Caption=""Shipper account"" msprop:dbcolumn=""shipperaccount"" msprop:expression="""" msprop:dbtable=""ngposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""streamcontract"" msdata:Caption=""Stream contract"" msprop:dbcolumn=""streamcontract"" msprop:expression="""" msprop:dbtable=""ngposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""24""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""streamrank"" msdata:Caption=""Stream rank"" msprop:dbcolumn=""streamrank"" msprop:expression="""" msprop:dbtable=""ngposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""priority"" msdata:Caption=""Priority"" msprop:dbcolumn=""priority"" msprop:expression="""" msprop:dbtable=""ngposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""pipelinecontract"" msdata:Caption=""Pipeline contract"" msprop:dbcolumn=""pipelinecontract"" msprop:expression="""" msprop:dbtable=""ngposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""24""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""service"" msprop:dbcolumn=""service"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""servicestatus"" msdata:Caption=""Service status"" msprop:dbcolumn=""servicestatus"" msprop:expression="""" msprop:viewdefault=""PRIMARY"" msprop:dbtable=""ngposition"" default=""PRIMARY"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""segment"" msdata:Caption=""Segment"" msprop:dbcolumn=""segment"" msprop:expression="""" msprop:dbtable=""ngposition"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""secondary"" msdata:Caption=""Secondary"" msprop:dbcolumn=""secondary"" msprop:expression="""" msprop:dbtable=""ngposition"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""firm"" msdata:Caption=""Firm"" msprop:dbcolumn=""firm"" msprop:expression="""" msprop:dbtable=""ngposition"" type=""xs:boolean"" default=""true"" minOccurs=""0""/>
											<xs:element name=""decint"" msprop:tradedetail_x003A_columnformat=""#,##0.00000000"" msprop:dbcolumn=""decint"" msprop:expression="""" msprop:viewdefault=""1"" msprop:dbtable="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""quantitymethod"" msdata:Caption=""Quantity method"" msprop:dbcolumn=""quantitymethod"" msprop:expression="""" msprop:dbtable=""ngposition"" default=""DELIVERY"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""evergreentermdate"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""evergreentermdate"" msprop:expression="""" msprop:dbtable="""" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""oba"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""allocpct"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""controlarea"" msdata:Caption=""Control area"" msprop:dbcolumn=""controlarea"" msprop:expression="""" msprop:dbtable=""powerposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""rank"" msdata:Caption=""Rank"" msprop:dbcolumn=""rank"" msprop:expression="""" msprop:dbtable=""powerposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""oasis"" msdata:Caption=""OASIS"" msprop:dbcolumn=""oasis"" msprop:expression="""" msprop:dbtable=""powerposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""transtatus"" msdata:Caption=""Tran status"" msprop:dbcolumn=""transtatus"" msprop:expression="""" msprop:dbtable=""powerposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""tsclass"" msdata:Caption=""TS class"" msprop:dbcolumn=""tsclass"" msprop:expression="""" msprop:dbtable=""powerposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""tsincrement"" msdata:Caption=""TS increment"" msprop:dbcolumn=""tsincrement"" msprop:expression="""" msprop:dbtable=""powerposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""tstype"" msdata:Caption=""TS type"" msprop:dbcolumn=""tstype"" msprop:expression="""" msprop:dbtable=""powerposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""losspct"" msprop:dbcolumn=""losspct"" msprop:expression="""" msprop:dbtable="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""paycongestion"" msdata:Caption=""Pay congestion"" msprop:dbcolumn=""paycongestion"" msprop:expression="""" msprop:dbtable=""powerposition"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""preconfirmed"" msdata:Caption=""Preconfirmed"" msprop:dbcolumn=""preconfirmed"" msprop:expression="""" msprop:dbtable=""powerposition"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""generationunit"" msdata:Caption=""Generation unit"" msprop:dbcolumn=""generationunit"" msprop:expression="""" msprop:dbtable=""powerposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""gencontrolarea"" msdata:Caption=""Generation control area"" msprop:dbcolumn=""gencontrolarea"" msprop:expression="""" msprop:dbtable=""powerposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""genpoint"" msdata:Caption=""Generation point"" msprop:dbcolumn=""genpoint"" msprop:expression="""" msprop:dbtable=""powerposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""genpct"" msdata:Caption=""Generation percent"" msprop:dbcolumn=""genpct"" msprop:expression="""" msprop:dbtable=""powerposition"" type=""xs:decimal"" default=""1"" minOccurs=""0""/>
											<xs:element name=""location"" msdata:Caption=""Location"" msprop:dbcolumn=""location"" msprop:expression="""" msprop:dbtable=""physicalposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""origin"" msdata:Caption=""Origin"" msprop:dbcolumn=""origin"" msprop:expression="""" msprop:dbtable=""physicalposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""destination"" msdata:Caption=""Destination"" msprop:dbcolumn=""destination"" msprop:expression="""" msprop:dbtable=""physicalposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""tank"" msdata:Caption=""Tank"" msprop:dbcolumn=""tank"" msprop:expression="""" msprop:dbtable=""physicalposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""pile"" msdata:Caption=""Pile"" msprop:dbcolumn=""pile"" msprop:expression="""" msprop:dbtable=""physicalposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""component"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""carrier"" msdata:Caption=""Carrier"" msprop:dbcolumn=""carrier"" msprop:expression="""" msprop:dbtable=""physicalposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""delmethod"" msdata:Caption=""Del method"" msprop:dbcolumn=""delmethod"" msprop:expression="""" msprop:dbtable=""physicalposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""incoterms"" msdata:Caption=""Incoterms"" msprop:dbcolumn=""incoterms"" msprop:expression="""" msprop:dbtable=""physicalposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""custodychain"" msdata:Caption=""Custody chain"" msprop:dbcolumn=""custodychain"" msprop:expression="""" msprop:dbtable=""physicalposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""128""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""forwardquantity"" msdata:Caption=""Position status"" msprop:dbcolumn=""forwardquantity"" msprop:expression="""" msprop:dbtable=""physicalposition"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""alternate"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""locationbasis"" msdata:Caption=""Location basis"" msprop:dbcolumn=""locationbasis"" msprop:expression="""" msprop:dbtable=""physicalposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""productbasis"" msdata:Caption=""Product basis"" msprop:dbcolumn=""productbasis"" msprop:expression="""" msprop:dbtable=""physicalposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""24""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""surrogate"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""quantitytype"" msprop:dbcolumn=""quantitytype"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""carriermode"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""shipment"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""cycle"" msprop:dbcolumn=""cycle"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""measure"" msdata:Caption=""Measure"" msprop:dbcolumn=""measure"" msprop:expression="""" msprop:dbtable=""emissionquantity"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""quantitystatus"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""posstatus"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""mass"" msdata:Caption=""Mass"" msprop:dbcolumn=""mass"" msprop:expression="""" msprop:dbtable=""physicalquantity"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""massunit"" msdata:Caption=""Mass unit"" msprop:dbcolumn=""massunit"" msprop:expression="""" msprop:dbtable=""physicalquantity"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""volume"" msprop:dbcolumn=""volume"" msprop:expression="""" msprop:dbtable="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""volumeunit"" msprop:dbcolumn=""volumeunit"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""gravity"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""gravityunit"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""altquantity"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""altunit"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""origshipment"" msdata:Caption=""Orig shipment"" msprop:dbcolumn=""origshipment"" msprop:expression="""" msprop:dbtable=""physicalquantity"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""sched"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""batchid"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""path"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""ofo"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""energy"" msprop:dbcolumn=""energy"" msprop:expression="""" msprop:dbtable="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""energyunit"" msprop:dbcolumn=""energyunit"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""qualityspec"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""lossmethod"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""losstype"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""daylightsaving"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""marketdayhour"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""timezone"" msprop:dbcolumn=""timezone"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""duration"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""tag"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""optionstatus"" msdata:Caption=""Option status"" msprop:dbcolumn=""optionstatus"" msprop:expression="""" msprop:dbtable=""emissionquantity"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""cutstatus"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""cutdate"" msdata:DateTimeMode=""Unspecified"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""tsperiod"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""poolschedtype"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""rampbeg"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""rampend"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""emissionprogram"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""emissionphase"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""vintageyear"" msdata:Caption=""Vintage year"" msprop:dbcolumn=""vintageyear"" msprop:expression="""" msprop:dbtable=""emissionposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""recstateprovince"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""rectype"" msdata:Caption=""REC type"" msprop:dbcolumn=""rectype"" msprop:expression="""" msprop:dbtable=""emissionposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""program"" msdata:Caption=""Program"" msprop:dbcolumn=""program"" msprop:expression="""" msprop:dbtable=""emissionposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""phase"" msdata:Caption=""Phase"" msprop:dbcolumn=""phase"" msprop:expression="""" msprop:dbtable=""emissionposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""certificatetype"" msdata:Caption=""Certification type"" msprop:dbcolumn=""certificatetype"" msprop:expression="""" msprop:dbtable=""emissionposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""accountname"" msdata:Caption=""Account name"" msprop:dbcolumn=""accountname"" msprop:expression="""" msprop:dbtable=""emissionquantity"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""accountnumber"" msdata:Caption=""Account number"" msprop:dbcolumn=""accountnumber"" msprop:expression="""" msprop:dbtable=""emissionquantity"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""startcertificatenumber"" msdata:Caption=""Start certificate number"" msprop:dbcolumn=""startcertificatenumber"" msprop:expression="""" msprop:dbtable=""emissionquantity"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""endcertificatenumber"" msdata:Caption=""End certificate number"" msprop:dbcolumn=""endcertificatenumber"" msprop:expression="""" msprop:dbtable=""emissionquantity"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""blocktotal"" msdata:Caption=""Block total"" msprop:dbcolumn=""blocktotal"" msprop:expression="""" msprop:dbtable=""emissionquantity"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""registry"" msdata:Caption=""Registry"" msprop:dbcolumn=""registry"" msprop:expression="""" msprop:dbtable=""emissionquantity"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""source"" msdata:Caption=""Source"" msprop:dbcolumn=""source"" msprop:expression="""" msprop:dbtable=""emissionquantity"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""titletransferdate"" msdata:DateTimeMode=""Unspecified"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""registryconfirmation"" msdata:Caption=""Registry confirmation"" msprop:dbcolumn=""registryconfirmation"" msprop:expression="""" msprop:dbtable=""emissionquantity"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""associateposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""generationdate"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""duedate"" msdata:Caption=""Due date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""duedate"" msprop:expression="""" msprop:dbtable=""emissionquantity"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""genunit"" msdata:Caption=""Generation unit"" msprop:dbcolumn=""genunit"" msprop:expression="""" msprop:dbtable=""emissionquantity"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""tradeoption"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""optionfunction"" msdata:Caption=""Option Function"" msprop:dbcolumn=""optionfunction"" msprop:expression="""" msprop:dbtable=""tradeoption"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""exerciseinstrument"" msdata:Caption=""Exercise Instrument"" msprop:dbcolumn=""exerciseinstrument"" msprop:expression="""" msprop:viewdefault=""CASH SETTLED"" msprop:dbtable=""tradeoption"" default=""CASH SETTLED"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""voltype"" msdata:Caption=""Volatility Type"" msprop:dbcolumn=""voltype"" msprop:expression="""" msprop:dbtable=""tradeoption"" default=""FLAT"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""forwardtype"" msdata:Caption=""Forward Type"" msprop:dbcolumn=""forwardtype"" msprop:expression="""" msprop:dbtable=""tradeoption"" default=""FLAT"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""discounttype"" msdata:Caption=""Discount Type"" msprop:dbcolumn=""discounttype"" msprop:expression="""" msprop:dbtable=""tradeoption"" default=""FLAT"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""maxexercisequantity"" msdata:Caption=""Max Exercise Quantity"" msprop:dbcolumn=""maxexercisequantity"" msprop:expression="""" msprop:dbtable=""tradeoption"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""quantityunit"" msdata:Caption=""Unit"" msprop:dbcolumn=""quantityunit"" msprop:expression="""" msprop:dbtable=""tradeoption"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""maxquantity"" msdata:Caption=""Max Quantity"" msprop:dbcolumn=""maxquantity"" msprop:expression="""" msprop:dbtable=""tradeoption"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""minquantity"" msdata:Caption=""Min Quantity"" msprop:dbcolumn=""minquantity"" msprop:expression="""" msprop:dbtable=""tradeoption"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""baseloadpct"" msdata:Caption=""Base Load Percent"" msprop:dbcolumn=""baseloadpct"" msprop:expression="""" msprop:dbtable=""tradeoption"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""swingrights"" msdata:Caption=""Swing Rights"" msprop:dbcolumn=""swingrights"" msprop:expression="""" msprop:dbtable=""tradeoption"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""underlyingpositionmode"" msdata:Caption=""Position mode"" msprop:dbcolumn=""underlyingpositionmode"" msprop:expression="""" msprop:dbtable=""tradeoption"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""underlyingoptionstyle"" msdata:Caption=""Option Style"" msprop:dbcolumn=""underlyingoptionstyle"" msprop:expression="""" msprop:dbtable=""tradeoption"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""underlyingoptionfrequency"" msdata:Caption=""Option Frequency"" msprop:dbcolumn=""underlyingoptionfrequency"" msprop:expression="""" msprop:dbtable=""tradeoption"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""underlyingexpirationdate"" msdata:Caption=""Expiration Date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""underlyingexpirationdate"" msprop:expression="""" msprop:dbtable=""tradeoption"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""energyfactor"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""volumeprecision"" type=""xs:short"" minOccurs=""0""/>
											<xs:element name=""energyprecision"" type=""xs:short"" minOccurs=""0""/>
											<xs:element name=""creationdate"" msdata:DateTimeMode=""Unspecified"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""creationname"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""position_h_version"" msdata:Caption=""Version"" msprop:dbcolumn=""h_version"" msprop:expression="""" msprop:dbtable=""position"" type=""xs:long"" default=""1"" minOccurs=""0""/>
											<xs:element name=""prodposition_h_version"" msprop:dbcolumn=""h_version"" msprop:expression="""" msprop:dbtable="""" type=""xs:long"" default=""-1"" minOccurs=""0""/>
											<xs:element name=""prodquantity_h_version"" msprop:dbcolumn=""h_version"" msprop:expression="""" msprop:dbtable="""" type=""xs:long"" default=""-1"" minOccurs=""0""/>
											<xs:element name=""symbol"" msprop:dbcolumn="""" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""contractperiod"" msprop:dbcolumn="""" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""ldcshipper"" msdata:Caption=""LDC shipper"" msprop:dbcolumn=""ldcshipper"" msprop:expression="""" msprop:dbtable=""ngposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""usestorage"" msdata:Caption=""Use shipper storage"" msprop:dbcolumn=""useshipperstorage"" msprop:expression="""" msprop:dbtable=""ngposition"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""totalquantity"" msprop:tradedetail_x003A_columnformat=""#,##0.00"" msprop:columnvisible=""false"" msprop:expression="""" msprop:dbtable="""" msprop:dbcolumn="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""fee_indexprecision"" msprop:dbtable=""feesettlement"" msprop:proxycolumn=""fee_indexprecision"" msprop:proxypane=""pricedetail"" msprop:viewdefault=""5"" msprop:expression="""" msprop:dbcolumn=""indexprecision"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""fee_priceindex"" msprop:proxypane=""pricedetail"" msprop:dbcolumn=""priceindex"" msprop:expression="""" msprop:proxycolumn=""fee_priceindex"" msprop:dbtable=""fee"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""fee_priceprecision"" msprop:dbtable=""feesettlement"" msprop:proxycolumn=""fee_priceprecision"" msprop:proxypane=""pricedetail"" msprop:viewdefault=""5"" msprop:expression="""" msprop:dbcolumn=""priceprecision"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""fee_pricediff"" msprop:dbtable=""fee"" msprop:proxycolumn=""fee_pricediff"" msprop:tradedetail_x003A_columnformat=""#,##0.00"" msprop:proxypane=""pricedetail"" msprop:viewdefault=""0"" msprop:expression="""" msprop:dbcolumn=""pricediff"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""pricelevel"" msprop:dbtable=""fee"" msprop:proxycolumn=""pricelevel"" msprop:proxypane=""pricedetail"" msprop:viewdefault=""AVG"" msprop:expression="""" msprop:dbcolumn=""pricelevel"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""timeperiod"" msprop:proxypane=""pricedetail"" msprop:dbcolumn=""timeperiod"" msprop:expression="""" msprop:proxycolumn=""timeperiod"" msprop:dbtable=""feetimeperiod"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""strike_unit"" msprop:proxypane=""pricedetail"" msprop:dbcolumn=""unit"" msprop:expression="""" msprop:proxycolumn=""strike_unit"" msprop:dbtable=""fee"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""deemgravity"" msprop:proxypane=""pricedetail"" msprop:dbcolumn=""deemgravity"" msprop:expression="""" msprop:proxycolumn=""deemgravity"" msprop:dbtable=""feeproduct"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""swingtolerance"" msdata:Caption=""Swing tolerance"" msprop:tradedetail_x003A_columnformat=""#,##0.00%"" msprop:dbcolumn=""swingtolerance"" msprop:expression="""" msprop:viewdefault=""0"" msprop:dbtable=""position"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""tsstage"" msdata:Caption=""Tsstage"" msprop:dbcolumn=""tsstage"" msprop:expression="""" msprop:dbtable=""powerposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""tsnumber"" msdata:Caption=""Tsnumber"" msprop:dbcolumn=""tsnumber"" msprop:expression="""" msprop:dbtable=""powerposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""tsnumberref"" msdata:Caption=""Tsnumber Ref"" msprop:dbcolumn=""tsnumberref"" msprop:expression="""" msprop:dbtable=""powerposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""entryexitbalancetype"" msdata:Caption=""Entryexit balancetype"" msprop:dbcolumn=""entryexitbalancetype"" msprop:expression="""" msprop:viewdefault=""PHYSICAL"" msprop:dbtable=""ngposition"" default=""PHYSICAL"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""entryexitcapacityrequired"" msdata:Caption=""Capacity required"" msprop:dbcolumn=""entryexitcapacityrequired"" msprop:expression="""" msprop:viewdefault=""0"" msprop:dbtable=""ngposition"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""quantitysyncreference"" msdata:Caption=""Quantitysync reference"" msprop:dbcolumn=""quantitysyncreference"" msprop:expression="""" msprop:dbtable=""ngposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""strike_pricediff"" msprop:dbtable=""fee"" msprop:proxycolumn=""strike_pricediff"" msprop:proxypane=""pricedetail"" msprop:viewdefault=""0"" msprop:expression="""" msprop:dbcolumn=""pricediff"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""evergreenforecast"" msdata:Caption=""Evergreen forecast"" msprop:dbcolumn=""evergreenforecast"" msprop:expression="""" msprop:dbtable=""trade"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""fee_currency"" msprop:dbtable=""fee"" msprop:proxycolumn=""fee_currency"" msprop:proxypane=""pricedetail"" msprop:viewdefault=""USD"" msprop:expression="""" msprop:dbcolumn=""currency"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""fee_feemethod"" msprop:dbtable=""fee"" msprop:proxycolumn=""fee_feemethod"" msprop:proxypane=""pricedetail"" msprop:viewdefault=""COMMODITY PRICE"" msprop:expression="""" msprop:dbcolumn=""feemethod"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""companyeic"" msdata:Caption=""EIC code"" msprop:dbcolumn=""companyeic"" msprop:expression="""" msprop:dbtable=""powerposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""counterpartyeic"" msdata:Caption=""EIC code"" msprop:dbcolumn=""counterpartyeic"" msprop:expression="""" msprop:dbtable=""powerposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""feemode"" msprop:proxypane=""pricedetail"" msprop:dbcolumn=""feemode"" msprop:expression="""" msprop:proxycolumn=""feemode"" msprop:dbtable=""fee"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""feetimeperiod"" msprop:proxypane=""pricedetail"" msprop:dbcolumn=""feetimeperiod"" msprop:expression="""" msprop:proxycolumn=""feetimeperiod"" msprop:dbtable=""fee"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""accttype"" msdata:Caption=""Account Type"" msprop:dbcolumn=""accttype"" msprop:expression="""" msprop:dbtable=""emissionquantity"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""swingconstraint"" msdata:Caption=""Swing constraint"" msprop:dbcolumn=""swingconstraint"" msprop:expression="""" msprop:dbtable=""tradeoption"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""constrainttimeunit"" msdata:Caption=""Constraint time unit"" msprop:dbcolumn=""constrainttimeunit"" msprop:expression="""" msprop:dbtable=""tradeoption"" default=""MONTH"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""33""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""cascadable"" msdata:Caption=""Cascadeable"" msprop:dbcolumn=""cascadable"" msprop:expression="""" msprop:dbtable=""finposition"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""otccalendar"" msprop:dbcolumn=""otccalendar"" msprop:expression="""" msprop:dbtable=""tradeoption"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""cascadedate"" msdata:Caption=""Cascade date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""cascadedate"" msprop:expression="""" msprop:dbtable=""finposition"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""settlephysically"" msdata:Caption=""Settle physically"" msprop:dbcolumn=""settlephysically"" msprop:expression="""" msprop:dbtable=""finposition"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""swaptype"" msdata:Caption=""Swap type"" msprop:dbcolumn=""swaptype"" msprop:expression="""" msprop:dbtable=""position"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""heatrate"" msprop:dbtable=""fee"" msprop:proxycolumn=""heatrate"" msprop:proxypane=""pricedetail"" msprop:viewdefault=""1"" msprop:expression="""" msprop:dbcolumn=""factor"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""ldcfirmquantity"" msdata:Caption=""Firm quantity"" msprop:dbcolumn=""ldcfirmquantity"" msprop:expression="""" msprop:dbtable=""ngposition"" msprop:tradedetail_x003A_columnformat=""#,##0.00"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""ldcfirmsource"" msdata:Caption=""Firm source"" msprop:dbcolumn=""ldcfirmsource"" msprop:expression="""" msprop:dbtable=""ngposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""ldcmeter"" msdata:Caption=""LDC meter"" msprop:dbcolumn=""ldcmeter"" msprop:expression="""" msprop:dbtable=""ngposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""useshipperstorage"" msdata:Caption=""Use shipper storage"" msprop:dbcolumn=""useshipperstorage"" msprop:expression="""" msprop:dbtable=""ngposition"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""demurragehours"" msdata:Caption=""Allowed laytime"" msprop:dbcolumn=""demurragehours"" msprop:expression="""" msprop:dbtable=""physicalposition"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""demurragerate"" msdata:Caption=""Demurrage rate"" msprop:dbcolumn=""demurragerate"" msprop:expression="""" msprop:dbtable=""physicalposition"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""demurragecurrency"" msdata:Caption=""Demurrage currency"" msprop:dbcolumn=""demurragecurrency"" msprop:expression="""" msprop:dbtable=""physicalposition"" default=""USD"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""demurrageunit"" msdata:Caption=""Demurrage unit"" msprop:dbcolumn=""demurrageunit"" msprop:expression="""" msprop:dbtable=""physicalposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""despatchrate"" msdata:Caption=""Despatch rate"" msprop:dbcolumn=""despatchrate"" msprop:expression="""" msprop:dbtable=""physicalposition"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""heatvalue"" msprop:dbcolumn=""heatvalue"" msprop:expression="""" msprop:dbtable="""" msprop:tradedetail_x003A_columnformat=""#,##0.0000"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""hvunit"" msprop:dbcolumn=""hvunit"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""gasplantcontract"" msdata:Caption=""Gasplant contract"" msprop:dbcolumn=""gasplantcontract"" msprop:expression="""" msprop:dbtable=""ngposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""24""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""counterpartyaccount"" msdata:Caption=""Counterparty account"" msprop:dbcolumn=""counterpartyaccount"" msprop:expression="""" msprop:dbtable=""ngposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""fee_postprice"" msprop:dbtable=""fee"" msprop:tradedetail_x003A_columnformat=""#,##0.00"" msprop:proxypane=""pricedetail"" msprop:expression="""" msprop:proxycolumn=""fee_postprice"" msprop:dbcolumn=""postprice"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""fee_postdate"" msdata:DateTimeMode=""Unspecified"" msprop:dbtable=""fee"" msprop:tradedetail_x003A_columnformat=""ShortDateTime"" msprop:proxypane=""pricedetail"" msprop:expression="""" msprop:proxycolumn=""fee_postdate"" msprop:dbcolumn=""postdate"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""fee_fixingdate"" msdata:DateTimeMode=""Unspecified"" msprop:dbtable=""fee"" msprop:tradedetail_x003A_columnformat=""ShortDateTime"" msprop:proxypane=""pricedetail"" msprop:expression="""" msprop:proxycolumn=""fee_fixingdate"" msprop:dbcolumn=""fixingdate"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""fee_unit"" msprop:proxypane=""pricedetail"" msprop:dbcolumn=""unit"" msprop:expression="""" msprop:proxycolumn=""fee_unit"" msprop:dbtable=""fee"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""bidtype"" msdata:Caption=""Bid type"" msprop:dbcolumn=""bidtype"" msprop:expression="""" msprop:dbtable=""powerposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""iso"" msdata:Caption=""ISO"" msprop:dbcolumn=""iso"" msprop:expression="""" msprop:dbtable=""powerposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""markettype"" msdata:Caption=""Market type"" msprop:dbcolumn=""markettype"" msprop:expression="""" msprop:dbtable=""powerposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""4""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""participant"" msdata:Caption=""Participant"" msprop:dbcolumn=""participant"" msprop:expression="""" msprop:dbtable=""powerposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""finposition_h_version"" msdata:Caption=""Version"" msprop:dbcolumn=""h_version"" msprop:expression="""" msprop:dbtable=""finposition"" type=""xs:int"" default=""1"" minOccurs=""0""/>
											<xs:element name=""balancegroup"" msdata:Caption=""Balance Group"" msprop:dbcolumn=""balancegroup"" msprop:expression="""" msprop:dbtable=""powerposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""delbalancegroup"" msdata:Caption=""Del Balance Group"" msprop:dbcolumn=""delbalancegroup"" msprop:expression="""" msprop:dbtable=""powerposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""recbalancegroup"" msdata:Caption=""Rec Balance Group"" msprop:dbcolumn=""recbalancegroup"" msprop:expression="""" msprop:dbtable=""powerposition"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""RowNumber"" type=""xs:int"" minOccurs=""0""/>
										</xs:sequence>
									</xs:complexType>
								</xs:element>
								<xs:element name=""quantitydetail"" msprop:ProxyVersion=""v2"" msprop:viewpane=""quantitydetail"">
									<xs:complexType>
										<xs:sequence>
											<xs:element name=""position"" type=""xs:string"" default=""""/>
											<xs:element name=""posdetail"" type=""xs:string"" default=""""/>
											<xs:element name=""surrogate"" msdata:AutoIncrement=""true"" msdata:AutoIncrementStep=""-1"" type=""xs:decimal""/>
											<xs:element name=""quantitytype"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""begtime"" msdata:DateTimeMode=""Unspecified"" msprop:dbtable="""" msprop:dbcolumn=""begtime"" msprop:quantitydetail_x003A_columnformat=""ShortDate"" msprop:expression="""" msprop:quantitydetail_x003A_sortorder=""ASC"" msprop:quantitydetail_x003A_sortseq=""1"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""endtime"" msdata:DateTimeMode=""Unspecified"" msprop:dbtable="""" msprop:dbcolumn=""endtime"" msprop:quantitydetail_x003A_columnformat=""ShortDate"" msprop:expression="""" msprop:quantitydetail_x003A_sortorder=""ASC"" msprop:quantitydetail_x003A_sortseq=""2"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""timezone"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""duration"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""tag"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""measure"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""quantitystatus"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""posstatus"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""optionstatus"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""cutstatus"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""cutdate"" msdata:DateTimeMode=""Unspecified"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""energy"" msprop:quantitydetail_x003A_columnformat=""#,##0.00"" msprop:dbcolumn=""energy"" msprop:expression="""" msprop:dbtable="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""energyunit"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""tsperiod"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""timeunit"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""poolschedtype"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""daylightsaving"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""offset"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE1"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE2"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE3"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE4"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE5"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE6"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE7"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE8"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE9"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE10"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE11"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE12"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE13"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE14"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE15"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE16"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE17"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE18"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE19"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE20"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE21"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE22"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE23"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE24"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE25"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE26"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE27"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE28"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE29"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE30"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE31"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE32"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE33"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE34"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE35"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE36"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE37"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE38"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE39"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE40"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE41"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE42"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE43"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE44"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE45"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE46"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE47"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE48"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE49"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE50"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE51"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE52"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE53"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE54"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE55"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE56"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE57"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE58"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE59"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE60"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE61"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE62"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE63"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE64"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE65"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE66"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE67"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE68"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE69"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE70"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE71"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE72"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE73"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE74"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE75"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE76"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE77"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE78"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE79"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE80"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE81"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE82"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE83"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE84"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE85"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE86"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE87"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE88"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE89"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE90"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE91"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE92"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE93"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE94"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE95"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE96"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE97"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE98"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE99"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HE100"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""price"" msprop:quantitydetail_x003A_columnformat=""#,##0.00"" msprop:dbcolumn="""" msprop:expression="""" msprop:dbtable="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP1"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP2"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP3"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP4"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP5"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP6"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP7"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP8"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP9"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP10"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP11"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP12"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP13"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP14"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP15"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP16"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP17"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP18"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP19"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP20"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP21"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP22"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP23"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP24"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP25"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP26"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP27"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP28"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP29"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP30"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP31"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP32"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP33"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP34"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP35"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP36"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP37"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP38"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP39"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP40"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP41"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP42"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP43"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP44"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP45"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP46"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP47"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP48"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP49"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP50"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP51"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP52"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP53"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP54"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP55"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP56"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP57"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP58"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP59"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP60"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP61"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP62"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP63"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP64"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP65"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP66"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP67"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP68"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP69"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP70"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP71"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP72"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP73"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP74"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP75"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP76"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP77"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP78"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP79"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP80"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP81"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP82"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP83"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP84"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP85"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP86"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP87"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP88"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP89"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP90"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP91"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP92"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP93"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP94"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP95"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP96"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP97"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP98"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP99"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""HP100"" msprop:expression="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""rampbeg"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""rampend"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""path"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""ofo"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""volume"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""volumeunit"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""qualityspec"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""lossmethod"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""losspct"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""losstype"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""cycle"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""marketdayhour"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""carriermode"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""shipment"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""mass"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""massunit"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""gravity"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""gravityunit"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""altquantity"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""altunit"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""origshipment"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""sched"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""batchid"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""accountname"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""accountnumber"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""startcertificatenumber"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""endcertificatenumber"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""blocktotal"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""registry"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""source"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""titletransferdate"" msdata:DateTimeMode=""Unspecified"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""registryconfirmation"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""associateposition"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""generationdate"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""duedate"" msdata:DateTimeMode=""Unspecified"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""genunit"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""RowNumber"" type=""xs:int"" minOccurs=""0""/>
										</xs:sequence>
									</xs:complexType>
								</xs:element>
								<xs:element name=""feedetail"" msprop:RecursiveChildColumn="""" msprop:ProxyVersion=""v2"" msprop:RecursiveColumn=""parentfee"" msprop:viewpane=""feedetail"">
									<xs:complexType>
										<xs:sequence>
											<xs:element name=""fee"" msdata:Caption=""Fee"" msprop:dbcolumn=""fee"" msprop:expression="""" msprop:dbtable=""fee"" msprop:tempsysgen=""0"" default="""">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""dbcolumn"" msdata:Caption=""Column"" msprop:dbcolumn=""dbcolumn"" msprop:expression="""" msprop:viewdefault=""POSITION"" msprop:dbtable=""fee"" default=""POSITION"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""dbvalue"" msdata:Caption=""Value"" msprop:dbcolumn=""dbvalue"" msprop:expression="""" msprop:dbtable=""fee"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""feemode"" msdata:Caption=""Fee mode"" msprop:dbcolumn=""feemode"" msprop:expression="""" msprop:viewdefault=""FIXED"" msprop:dbtable=""fee"" default=""FIXED"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""feetype"" msdata:Caption=""Fee type"" msprop:dbcolumn=""feetype"" msprop:expression="""" msprop:dbtable=""fee"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""description"" msdata:Caption=""Description"" msprop:dbcolumn=""description"" msprop:expression="""" msprop:dbtable=""fee"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""company"" msdata:Caption=""Company"" msprop:dbcolumn=""company"" msprop:expression="""" msprop:viewdefault=""VISTA"" msprop:dbtable=""fee"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""counterparty"" msdata:Caption=""Counterparty"" msprop:dbcolumn=""counterparty"" msprop:expression="""" msprop:dbtable=""fee"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""paystatus"" msdata:Caption=""Swap status"" msprop:dbcolumn=""paystatus"" msprop:expression="""" msprop:dbtable=""fee"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""4""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""optionpricetype"" msdata:Caption=""Option price type"" msprop:dbcolumn=""optionpricetype"" msprop:expression="""" msprop:dbtable=""fee"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""feecode"" msdata:Caption=""Fee code"" msprop:dbcolumn=""feecode"" msprop:expression="""" msprop:dbtable=""fee"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""unit"" msdata:Caption=""Unit"" msprop:dbcolumn=""unit"" msprop:expression="""" msprop:dbtable=""fee"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""priceindex"" msdata:Caption=""Price index"" msprop:dbcolumn=""priceindex"" msprop:expression="""" msprop:dbtable=""fee"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""pricelevel"" msdata:Caption=""Price level"" msprop:dbcolumn=""pricelevel"" msprop:expression="""" msprop:viewdefault=""AVG"" msprop:dbtable=""fee"" default=""AVG"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""pricediff"" msdata:Caption=""Price diff"" msprop:feedetail_x003A_columnformat=""#,##0.0000"" msprop:dbcolumn=""pricediff"" msprop:expression="""" msprop:viewdefault=""0"" msprop:dbtable=""fee"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""currency"" msdata:Caption=""Currency"" msprop:dbcolumn=""currency"" msprop:expression="""" msprop:dbtable=""fee"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""feemethod"" msdata:Caption=""Fee method"" msprop:dbcolumn=""feemethod"" msprop:expression="""" msprop:dbtable=""fee"" default=""DELVOLUME"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""factor"" msdata:Caption=""Factor"" msprop:feedetail_x003A_columnformat=""#,##0.00000000"" msprop:dbcolumn=""factor"" msprop:expression="""" msprop:viewdefault=""1"" msprop:dbtable=""fee"" type=""xs:decimal"" default=""1"" minOccurs=""0""/>
											<xs:element name=""quantityfactor"" msdata:Caption=""Quantity factor"" msprop:feedetail_x003A_columnformat=""#,##0.00000000"" msprop:dbcolumn=""quantityfactor"" msprop:expression="""" msprop:dbtable=""fee"" type=""xs:decimal"" default=""1"" minOccurs=""0""/>
											<xs:element name=""indexfactor"" msdata:Caption=""Index factor"" msprop:feedetail_x003A_columnformat=""#,##0.00000000"" msprop:dbcolumn=""indexfactor"" msprop:expression="""" msprop:viewdefault=""1"" msprop:dbtable=""fee"" type=""xs:decimal"" default=""1"" minOccurs=""0""/>
											<xs:element name=""feetier"" msdata:Caption=""Tier"" msprop:dbcolumn=""feetier"" msprop:expression="""" msprop:dbtable=""fee"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""feeproduct"" msdata:Caption=""Product"" msprop:dbcolumn=""feeproduct"" msprop:expression="""" msprop:dbtable=""fee"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""feetimeperiod"" msdata:Caption=""Time period"" msprop:dbcolumn=""feetimeperiod"" msprop:expression="""" msprop:dbtable=""fee"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""feesettlement"" msdata:Caption=""Settlement"" msprop:dbcolumn=""feesettlement"" msprop:expression="""" msprop:dbtable=""fee"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""feetag"" msdata:Caption=""Fee Tag"" msprop:dbcolumn=""feetag"" msprop:expression="""" msprop:dbtable=""fee"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""functiontag"" msdata:Caption=""Function Tag"" msprop:dbcolumn=""functiontag"" msprop:expression="""" msprop:dbtable=""fee"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""parentfee"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""internalfee"" msdata:Caption=""Internal Fee"" msprop:dbcolumn=""internalfee"" msprop:expression="""" msprop:dbtable=""fee"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""surrogate"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""product"" msdata:Caption=""Product"" msprop:feedetail_x003A_columnformat=""boolean"" msprop:dbcolumn=""product"" msprop:expression="""" msprop:dbtable=""feeproduct"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""24""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""component"" msdata:Caption=""Component"" msprop:dbcolumn=""component"" msprop:expression="""" msprop:dbtable=""feeproduct"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""24""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""ancquantity"" msdata:Caption=""Anc quantity"" msprop:feedetail_x003A_columnformat=""#,##0.000000"" msprop:dbcolumn=""ancquantity"" msprop:expression="""" msprop:dbtable=""feeproduct"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""minquantity"" msdata:Caption=""Min quantity"" msprop:feedetail_x003A_columnformat=""#,##0.0000"" msprop:dbcolumn=""minquantity"" msprop:expression="""" msprop:dbtable=""feeproduct"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""maxquantity"" msdata:Caption=""Max quantity"" msprop:feedetail_x003A_columnformat=""#,##0.0000"" msprop:dbcolumn=""maxquantity"" msprop:expression="""" msprop:dbtable=""feeproduct"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""minpct"" msdata:Caption=""Min Pct"" msprop:feedetail_x003A_columnformat="".00"" msprop:dbcolumn=""minpct"" msprop:expression="""" msprop:dbtable=""feeproduct"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""maxpct"" msdata:Caption=""Max Pct"" msprop:feedetail_x003A_columnformat="".00"" msprop:dbcolumn=""maxpct"" msprop:expression="""" msprop:dbtable=""feeproduct"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""pctcomponent"" msdata:Caption=""Pct Component"" msprop:dbcolumn=""pctcomponent"" msprop:expression="""" msprop:dbtable=""feeproduct"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""location"" msdata:Caption=""Location"" msprop:dbcolumn=""location"" msprop:expression="""" msprop:dbtable=""feeproduct"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""deemgravity"" msdata:Caption=""Deem gravity"" msprop:dbcolumn=""deemgravity"" msprop:expression="""" msprop:dbtable=""feeproduct"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""stdgravity"" msdata:Caption=""Std gravity"" msprop:dbcolumn=""stdgravity"" msprop:expression="""" msprop:dbtable=""feeproduct"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""gravityunit"" msdata:Caption=""Gravity unit"" msprop:dbcolumn=""gravityunit"" msprop:expression="""" msprop:dbtable=""feeproduct"" default=""API"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""4""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""regradediff"" msdata:Caption=""Regrade Diff"" msprop:dbcolumn=""regradediff"" msprop:expression="""" msprop:dbtable=""feeproduct"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""escalation"" msdata:Caption=""Escalation"" msprop:dbcolumn=""escalation"" msprop:expression="""" msprop:dbtable=""feeproduct"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""contractcategory"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""indexprecision"" msdata:Caption=""Price precision"" msprop:feedetail_x003A_columnformat=""#,##0"" msprop:dbcolumn=""indexprecision"" msprop:expression="""" msprop:viewdefault=""5"" msprop:dbtable=""feesettlement"" type=""xs:decimal"" default=""5"" minOccurs=""0""/>
											<xs:element name=""priceprecision"" msdata:Caption=""Price precision"" msprop:feedetail_x003A_columnformat=""#,##0"" msprop:dbcolumn=""priceprecision"" msprop:expression="""" msprop:viewdefault=""5"" msprop:dbtable=""feesettlement"" type=""xs:decimal"" default=""5"" minOccurs=""0""/>
											<xs:element name=""roundmethod"" msdata:Caption=""Round method"" msprop:dbcolumn=""roundmethod"" msprop:expression="""" msprop:dbtable=""feesettlement"" default=""UP"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""paymentterms"" msdata:Caption=""Payment terms"" msprop:dbcolumn=""paymentterms"" msprop:expression="""" msprop:dbtable=""feesettlement"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""taxable"" msdata:Caption=""Taxable"" msprop:feedetail_x003A_columnformat=""Boolean"" msprop:dbcolumn=""taxable"" msprop:expression="""" msprop:dbtable=""feesettlement"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""taxlocation"" msdata:Caption=""Tax Location"" msprop:dbcolumn=""taxlocation"" msprop:expression="""" msprop:dbtable=""feesettlement"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""invoice"" msdata:Caption=""Invoice"" msprop:dbcolumn=""invoice"" msprop:expression="""" msprop:dbtable=""feesettlement"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""invoicedate"" msdata:Caption=""Invoice date"" msdata:DateTimeMode=""Unspecified"" msprop:feedetail_x003A_columnformat=""ShortDate"" msprop:dbcolumn=""invoicedate"" msprop:expression="""" msprop:dbtable=""feesettlement"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""interestindex"" msdata:Caption=""Interest index"" msprop:dbcolumn=""interestindex"" msprop:expression="""" msprop:dbtable=""feesettlement"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""discountfactor"" msdata:Caption=""Discount Factor"" msprop:dbcolumn=""discountfactor"" msprop:expression="""" msprop:dbtable=""feesettlement"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""dayconvention"" msdata:Caption=""Day convention"" msprop:dbcolumn=""dayconvention"" msprop:expression="""" msprop:dbtable=""feesettlement"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""interest"" msdata:Caption=""Interest"" msprop:dbcolumn=""interest"" msprop:expression="""" msprop:dbtable=""feesettlement"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""interestrate"" msdata:Caption=""Interest Rate"" msprop:dbcolumn=""interestrate"" msprop:expression="""" msprop:dbtable=""feesettlement"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""spreadreference"" msdata:Caption=""SpreadReference"" msprop:feedetail_x003A_columnformat=""Boolean"" msprop:dbcolumn=""spreadreference"" msprop:expression="""" msprop:viewdefault=""0"" msprop:dbtable=""feesettlement"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""feestatus"" msdata:Caption=""Fee status"" msprop:dbcolumn=""feestatus"" msprop:expression="""" msprop:dbtable=""feesettlement"" default=""ACTUAL"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""aggregateshipment"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""aggregatemeasure"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""feecontract"" msdata:Caption=""Contract"" msprop:dbcolumn=""feecontract"" msprop:expression="""" msprop:dbtable=""feesettlement"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""plantquantitytype"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""tier"" msdata:Caption=""Tier"" msprop:dbcolumn=""tier"" msprop:expression="""" msprop:dbtable=""feetier"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""tiermethod"" msdata:Caption=""Tier method"" msprop:dbcolumn=""tiermethod"" msprop:expression="""" msprop:dbtable=""feetier"" default=""POSDETAIL"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""tierquantity"" msdata:Caption=""Tier quantity"" msprop:feedetail_x003A_columnformat=""#,##0.0"" msprop:dbcolumn=""tierquantity"" msprop:expression="""" msprop:dbtable=""feetier"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""tiertimeunit"" msdata:Caption=""Tier time unit"" msprop:dbcolumn=""tiertimeunit"" msprop:expression="""" msprop:dbtable=""feetier"" default=""DAY"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""contracttier"" msdata:Caption=""Contract tier"" msprop:dbcolumn=""contracttier"" msprop:expression="""" msprop:dbtable=""feetier"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""triggerparty"" msdata:Caption=""Trigger party"" msprop:dbcolumn=""triggerparty"" msprop:expression="""" msprop:dbtable=""feetier"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""triggerstatus"" msdata:Caption=""Trigger status"" msprop:dbcolumn=""triggerstatus"" msprop:expression="""" msprop:dbtable=""feetier"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""postprice"" msdata:Caption=""Post price"" msprop:dbcolumn=""postprice"" msprop:expression="""" msprop:dbtable=""fee"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""tiertype"" msdata:Caption=""Tier Type"" msprop:dbcolumn=""tiertype"" msprop:expression="""" msprop:dbtable=""feetier"" default=""QUANTITY"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""tierunit"" msdata:Caption=""Tier Unit"" msprop:dbcolumn=""tierunit"" msprop:expression="""" msprop:dbtable=""feetier"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""begtime"" msdata:Caption=""Beg time"" msdata:DateTimeMode=""Unspecified"" msprop:feedetail_x003A_columnformat=""ShortDateTime"" msprop:dbcolumn=""begtime"" msprop:expression="""" msprop:dbtable=""feetimeperiod"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""endtime"" msdata:Caption=""End time"" msdata:DateTimeMode=""Unspecified"" msprop:feedetail_x003A_columnformat=""ShortDateTime"" msprop:dbcolumn=""endtime"" msprop:expression="""" msprop:dbtable=""feetimeperiod"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""timeperiod"" msdata:Caption=""Time period"" msprop:dbcolumn=""timeperiod"" msprop:expression="""" msprop:dbtable=""feetimeperiod"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""timeperiodbegtime"" msdata:Caption=""Time period beg time"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""timeperiodbegtime"" msprop:expression="""" msprop:dbtable=""feetimeperiod"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""timeperiodendtime"" msdata:Caption=""Time period end time"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""timeperiodendtime"" msprop:expression="""" msprop:dbtable=""feetimeperiod"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""timeperioddaytype"" msdata:Caption=""Time periodday type"" msprop:dbcolumn=""timeperioddaytype"" msprop:expression="""" msprop:dbtable=""feetimeperiod"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""priceevent"" msdata:Caption=""Price event"" msprop:dbcolumn=""priceevent"" msprop:expression="""" msprop:dbtable=""feetimeperiod"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""eventdate"" msdata:Caption=""Event date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""eventdate"" msprop:expression="""" msprop:dbtable=""feetimeperiod"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""timemethod"" msdata:Caption=""Time method"" msprop:dbcolumn=""timemethod"" msprop:expression="""" msprop:dbtable=""feetimeperiod"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""tsperiod"" msdata:Caption=""TS period"" msprop:dbcolumn=""tsperiod"" msprop:expression="""" msprop:dbtable=""feetimeperiod"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""promptoffsetdays"" msdata:Caption=""Prompt offset days"" msprop:dbcolumn=""promptoffsetdays"" msprop:expression="""" msprop:dbtable=""feetimeperiod"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""promptmonth"" msdata:Caption=""Prompt month"" msprop:dbcolumn=""promptmonth"" msprop:expression="""" msprop:dbtable=""feetimeperiod"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""currencytimeperiod"" msdata:Caption=""Currency timeperiod"" msprop:dbcolumn=""currencytimeperiod"" msprop:expression="""" msprop:dbtable=""feetimeperiod"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""rollmonth"" msdata:Caption=""Roll Month"" msprop:dbcolumn=""rollmonth"" msprop:expression="""" msprop:dbtable=""feetimeperiod"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""currencyresolutiontype"" msdata:Caption=""Currency Resolution Type"" msprop:dbcolumn=""currencyresolutiontype"" msprop:expression="""" msprop:dbtable=""feetimeperiod"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""dsthandling"" msdata:Caption=""DST Handling"" msprop:dbcolumn=""dsthandling"" msprop:expression="""" msprop:dbtable=""feetimeperiod"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""fee_id"" type=""xs:int"" minOccurs=""0""/>
											<xs:element name=""creationname"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""creationdate"" msdata:DateTimeMode=""Unspecified"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""fee_h_version"" msdata:Caption=""Version"" msprop:dbcolumn=""h_version"" msprop:expression="""" msprop:dbtable=""fee"" type=""xs:long"" default=""1"" minOccurs=""0""/>
											<xs:element name=""feeproduct_h_version"" msdata:Caption=""Version"" msprop:dbcolumn=""h_version"" msprop:expression="""" msprop:dbtable=""feeproduct"" type=""xs:long"" default=""1"" minOccurs=""0""/>
											<xs:element name=""feetimeperiod_h_version"" msdata:Caption=""Version"" msprop:dbcolumn=""h_version"" msprop:expression="""" msprop:dbtable=""feetimeperiod"" type=""xs:long"" default=""1"" minOccurs=""0""/>
											<xs:element name=""feesettlement_h_version"" msdata:Caption=""Version"" msprop:dbcolumn=""h_version"" msprop:expression="""" msprop:dbtable=""feesettlement"" type=""xs:long"" default=""1"" minOccurs=""0""/>
											<xs:element name=""feetier_h_version"" msdata:Caption=""Version"" msprop:dbcolumn=""h_version"" msprop:expression="""" msprop:dbtable=""feetier"" type=""xs:long"" default=""1"" minOccurs=""0""/>
											<xs:element name=""RowNumber"" type=""xs:int"" minOccurs=""0""/>
											<xs:element name=""fixingdate"" msdata:DateTimeMode=""Unspecified"" msprop:dbtable=""fee"" msprop:dbcolumn=""fixingdate"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""postdate"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""postdate"" msprop:pricedetail_x003A_columnformat=""ShortDateTime"" msprop:dbtable=""fee"" type=""xs:dateTime"" minOccurs=""0""/>
										</xs:sequence>
									</xs:complexType>
								</xs:element>
								<xs:element name=""pricedetail"" msprop:RecursiveChildColumn="""" msprop:ProxyVersion=""v2"" msprop:RecursiveColumn=""parentfee"" msprop:viewpane=""feedetail"">
									<xs:complexType>
										<xs:sequence>
											<xs:element name=""fee"" msdata:Caption=""Fee"" msprop:dbcolumn=""fee"" msprop:expression="""" msprop:dbtable=""fee"" msprop:tempsysgen=""0"" default="""">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""dbcolumn"" msdata:Caption=""Column"" msprop:dbcolumn=""dbcolumn"" msprop:expression="""" msprop:viewdefault=""POSITION"" msprop:dbtable=""fee"" default=""POSITION"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""dbvalue"" msdata:Caption=""Value"" msprop:dbcolumn=""dbvalue"" msprop:expression="""" msprop:dbtable=""fee"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""feemode"" msdata:Caption=""Fee mode"" msprop:dbcolumn=""feemode"" msprop:expression="""" msprop:dbtable=""fee"" default=""FIXED"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""feetype"" msdata:Caption=""Fee type"" msprop:dbcolumn=""feetype"" msprop:expression="""" msprop:dbtable=""fee"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""description"" msdata:Caption=""Description"" msprop:dbcolumn=""description"" msprop:expression="""" msprop:dbtable=""fee"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""company"" msdata:Caption=""Company"" msprop:dbcolumn=""company"" msprop:expression="""" msprop:dbtable=""fee"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""counterparty"" msdata:Caption=""Counterparty"" msprop:dbcolumn=""counterparty"" msprop:expression="""" msprop:dbtable=""fee"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""paystatus"" msdata:Caption=""Swap status"" msprop:dbcolumn=""paystatus"" msprop:expression="""" msprop:dbtable=""fee"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""4""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""optionpricetype"" msdata:Caption=""Option price type"" msprop:dbcolumn=""optionpricetype"" msprop:expression="""" msprop:dbtable=""fee"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""feecode"" msdata:Caption=""Fee code"" msprop:dbcolumn=""feecode"" msprop:expression="""" msprop:dbtable=""fee"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""unit"" msdata:Caption=""Unit"" msprop:dbcolumn=""unit"" msprop:expression="""" msprop:dbtable=""fee"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""priceindex"" msdata:Caption=""Price index"" msprop:dbcolumn=""priceindex"" msprop:expression="""" msprop:dbtable=""fee"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""pricelevel"" msdata:Caption=""Price level"" msprop:dbcolumn=""pricelevel"" msprop:expression="""" msprop:viewdefault=""AVG"" msprop:dbtable=""fee"" default=""AVG"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""pricediff"" msdata:Caption=""Price diff"" msprop:dbtable=""fee"" msprop:pricedetail_x003A_columnformat=""#,##0.0000"" msprop:feedetail_x003A_columnformat=""#,##0.0000"" msprop:viewdefault=""0"" msprop:expression="""" msprop:dbcolumn=""pricediff"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""currency"" msdata:Caption=""Currency"" msprop:dbcolumn=""currency"" msprop:expression="""" msprop:viewdefault=""USD"" msprop:dbtable=""fee"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""feemethod"" msdata:Caption=""Fee method"" msprop:dbcolumn=""feemethod"" msprop:expression="""" msprop:viewdefault=""COMMODITY PRICE"" msprop:dbtable=""fee"" default=""DELVOLUME"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""factor"" msdata:Caption=""Factor"" msprop:dbtable=""fee"" msprop:pricedetail_x003A_columnformat=""#,##0.00000000"" msprop:feedetail_x003A_columnformat=""#,##0.00000000"" msprop:viewdefault=""1"" msprop:expression="""" msprop:dbcolumn=""factor"" type=""xs:decimal"" default=""1"" minOccurs=""0""/>
											<xs:element name=""quantityfactor"" msdata:Caption=""Quantity factor"" msprop:feedetail_x003A_columnformat=""#,##0.00000000"" msprop:dbcolumn=""quantityfactor"" msprop:expression="""" msprop:pricedetail_x003A_columnformat=""#,##0.00000000"" msprop:dbtable=""fee"" type=""xs:decimal"" default=""1"" minOccurs=""0""/>
											<xs:element name=""indexfactor"" msdata:Caption=""Index factor"" msprop:dbtable=""fee"" msprop:pricedetail_x003A_columnformat=""#,##0.00000000"" msprop:feedetail_x003A_columnformat=""#,##0.00000000"" msprop:viewdefault=""1"" msprop:expression="""" msprop:dbcolumn=""indexfactor"" type=""xs:decimal"" default=""1"" minOccurs=""0""/>
											<xs:element name=""feetier"" msdata:Caption=""Tier"" msprop:dbcolumn=""feetier"" msprop:expression="""" msprop:dbtable=""fee"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""feeproduct"" msdata:Caption=""Product"" msprop:dbcolumn=""feeproduct"" msprop:expression="""" msprop:dbtable=""fee"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""feetimeperiod"" msdata:Caption=""Time period"" msprop:dbcolumn=""feetimeperiod"" msprop:expression="""" msprop:dbtable=""fee"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""feesettlement"" msdata:Caption=""Settlement"" msprop:dbcolumn=""feesettlement"" msprop:expression="""" msprop:dbtable=""fee"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""feetag"" msdata:Caption=""Fee Tag"" msprop:dbcolumn=""feetag"" msprop:expression="""" msprop:dbtable=""fee"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""functiontag"" msdata:Caption=""Function Tag"" msprop:dbcolumn=""functiontag"" msprop:expression="""" msprop:dbtable=""fee"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""parentfee"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""internalfee"" msdata:Caption=""Internal Fee"" msprop:dbcolumn=""internalfee"" msprop:expression="""" msprop:dbtable=""fee"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""surrogate"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""product"" msdata:Caption=""Product"" msprop:feedetail_x003A_columnformat=""boolean"" msprop:dbcolumn=""product"" msprop:expression="""" msprop:pricedetail_x003A_columnformat=""boolean"" msprop:dbtable=""feeproduct"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""24""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""component"" msdata:Caption=""Component"" msprop:dbcolumn=""component"" msprop:expression="""" msprop:dbtable=""feeproduct"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""24""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""ancquantity"" msdata:Caption=""Anc quantity"" msprop:feedetail_x003A_columnformat=""#,##0.000000"" msprop:dbcolumn=""ancquantity"" msprop:expression="""" msprop:pricedetail_x003A_columnformat=""#,##0.000000"" msprop:dbtable=""feeproduct"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""minquantity"" msdata:Caption=""Min quantity"" msprop:feedetail_x003A_columnformat=""#,##0.0000"" msprop:dbcolumn=""minquantity"" msprop:expression="""" msprop:pricedetail_x003A_columnformat=""#,##0.0000"" msprop:dbtable=""feeproduct"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""maxquantity"" msdata:Caption=""Max quantity"" msprop:feedetail_x003A_columnformat=""#,##0.0000"" msprop:dbcolumn=""maxquantity"" msprop:expression="""" msprop:pricedetail_x003A_columnformat=""#,##0.0000"" msprop:dbtable=""feeproduct"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""minpct"" msdata:Caption=""Min Pct"" msprop:feedetail_x003A_columnformat="".00"" msprop:dbcolumn=""minpct"" msprop:expression="""" msprop:pricedetail_x003A_columnformat="".00"" msprop:dbtable=""feeproduct"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""maxpct"" msdata:Caption=""Max Pct"" msprop:feedetail_x003A_columnformat="".00"" msprop:dbcolumn=""maxpct"" msprop:expression="""" msprop:pricedetail_x003A_columnformat="".00"" msprop:dbtable=""feeproduct"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""pctcomponent"" msdata:Caption=""Pct Component"" msprop:dbcolumn=""pctcomponent"" msprop:expression="""" msprop:dbtable=""feeproduct"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""location"" msdata:Caption=""Location"" msprop:dbcolumn=""location"" msprop:expression="""" msprop:dbtable=""feeproduct"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""deemgravity"" msdata:Caption=""Deem gravity"" msprop:dbcolumn=""deemgravity"" msprop:expression="""" msprop:dbtable=""feeproduct"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""stdgravity"" msdata:Caption=""Std gravity"" msprop:dbcolumn=""stdgravity"" msprop:expression="""" msprop:dbtable=""feeproduct"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""gravityunit"" msdata:Caption=""Gravity unit"" msprop:dbcolumn=""gravityunit"" msprop:expression="""" msprop:dbtable=""feeproduct"" default=""API"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""4""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""regradediff"" msdata:Caption=""Regrade Diff"" msprop:dbcolumn=""regradediff"" msprop:expression="""" msprop:dbtable=""feeproduct"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""escalation"" msdata:Caption=""Escalation"" msprop:dbcolumn=""escalation"" msprop:expression="""" msprop:dbtable=""feeproduct"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""contractcategory"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""indexprecision"" msdata:Caption=""Price precision"" msprop:dbtable=""feesettlement"" msprop:pricedetail_x003A_columnformat=""#,##0"" msprop:feedetail_x003A_columnformat=""#,##0"" msprop:viewdefault=""5"" msprop:expression="""" msprop:dbcolumn=""indexprecision"" type=""xs:decimal"" default=""5"" minOccurs=""0""/>
											<xs:element name=""priceprecision"" msdata:Caption=""Price precision"" msprop:dbtable=""feesettlement"" msprop:pricedetail_x003A_columnformat=""#,##0"" msprop:feedetail_x003A_columnformat=""#,##0"" msprop:viewdefault=""5"" msprop:expression="""" msprop:dbcolumn=""priceprecision"" type=""xs:decimal"" default=""5"" minOccurs=""0""/>
											<xs:element name=""roundmethod"" msdata:Caption=""Round method"" msprop:dbcolumn=""roundmethod"" msprop:expression="""" msprop:dbtable=""feesettlement"" default=""UP"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""paymentterms"" msdata:Caption=""Payment terms"" msprop:dbcolumn=""paymentterms"" msprop:expression="""" msprop:dbtable=""feesettlement"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""taxable"" msdata:Caption=""Taxable"" msprop:feedetail_x003A_columnformat=""Boolean"" msprop:dbcolumn=""taxable"" msprop:expression="""" msprop:pricedetail_x003A_columnformat=""Boolean"" msprop:dbtable=""feesettlement"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""taxlocation"" msdata:Caption=""Tax Location"" msprop:dbcolumn=""taxlocation"" msprop:expression="""" msprop:dbtable=""feesettlement"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""invoice"" msdata:Caption=""Invoice"" msprop:dbcolumn=""invoice"" msprop:expression="""" msprop:dbtable=""feesettlement"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""invoicedate"" msdata:Caption=""Invoice date"" msdata:DateTimeMode=""Unspecified"" msprop:feedetail_x003A_columnformat=""ShortDate"" msprop:dbcolumn=""invoicedate"" msprop:expression="""" msprop:pricedetail_x003A_columnformat=""ShortDate"" msprop:dbtable=""feesettlement"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""interestindex"" msdata:Caption=""Interest index"" msprop:dbcolumn=""interestindex"" msprop:expression="""" msprop:dbtable=""feesettlement"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""discountfactor"" msdata:Caption=""Discount Factor"" msprop:dbcolumn=""discountfactor"" msprop:expression="""" msprop:dbtable=""feesettlement"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""dayconvention"" msdata:Caption=""Day convention"" msprop:dbcolumn=""dayconvention"" msprop:expression="""" msprop:dbtable=""feesettlement"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""interest"" msdata:Caption=""Interest"" msprop:dbcolumn=""interest"" msprop:expression="""" msprop:dbtable=""feesettlement"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""interestrate"" msdata:Caption=""Interest Rate"" msprop:dbcolumn=""interestrate"" msprop:expression="""" msprop:dbtable=""feesettlement"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""spreadreference"" msdata:Caption=""SpreadReference"" msprop:dbtable=""feesettlement"" msprop:pricedetail_x003A_columnformat=""Boolean"" msprop:feedetail_x003A_columnformat=""Boolean"" msprop:viewdefault=""0"" msprop:expression="""" msprop:dbcolumn=""spreadreference"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""feestatus"" msdata:Caption=""Fee status"" msprop:dbcolumn=""feestatus"" msprop:expression="""" msprop:dbtable=""feesettlement"" default=""ACTUAL"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""aggregateshipment"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""aggregatemeasure"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""feecontract"" msdata:Caption=""Contract"" msprop:dbcolumn=""feecontract"" msprop:expression="""" msprop:dbtable=""feesettlement"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""plantquantitytype"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""tier"" msdata:Caption=""Tier"" msprop:dbcolumn=""tier"" msprop:expression="""" msprop:dbtable=""feetier"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""tiermethod"" msdata:Caption=""Tier method"" msprop:dbcolumn=""tiermethod"" msprop:expression="""" msprop:dbtable=""feetier"" default=""POSDETAIL"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""tierquantity"" msdata:Caption=""Tier quantity"" msprop:feedetail_x003A_columnformat=""#,##0.0"" msprop:dbcolumn=""tierquantity"" msprop:expression="""" msprop:pricedetail_x003A_columnformat=""#,##0.0"" msprop:dbtable=""feetier"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""tiertimeunit"" msdata:Caption=""Tier time unit"" msprop:dbcolumn=""tiertimeunit"" msprop:expression="""" msprop:dbtable=""feetier"" default=""DAY"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""contracttier"" msdata:Caption=""Contract tier"" msprop:dbcolumn=""contracttier"" msprop:expression="""" msprop:dbtable=""feetier"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""triggerparty"" msdata:Caption=""Trigger party"" msprop:dbcolumn=""triggerparty"" msprop:expression="""" msprop:dbtable=""feetier"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""triggerstatus"" msdata:Caption=""Trigger status"" msprop:dbcolumn=""triggerstatus"" msprop:expression="""" msprop:dbtable=""feetier"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""postprice"" msdata:Caption=""Post price"" msprop:dbcolumn=""postprice"" msprop:expression="""" msprop:dbtable=""fee"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""tiertype"" msdata:Caption=""Tier Type"" msprop:dbcolumn=""tiertype"" msprop:expression="""" msprop:dbtable=""feetier"" default=""QUANTITY"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""tierunit"" msdata:Caption=""Tier Unit"" msprop:dbcolumn=""tierunit"" msprop:expression="""" msprop:dbtable=""feetier"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""begtime"" msdata:Caption=""Beg time"" msdata:DateTimeMode=""Unspecified"" msprop:feedetail_x003A_columnformat=""ShortDateTime"" msprop:dbcolumn=""begtime"" msprop:expression="""" msprop:pricedetail_x003A_columnformat=""ShortDateTime"" msprop:dbtable=""feetimeperiod"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""endtime"" msdata:Caption=""End time"" msdata:DateTimeMode=""Unspecified"" msprop:feedetail_x003A_columnformat=""ShortDateTime"" msprop:dbcolumn=""endtime"" msprop:expression="""" msprop:pricedetail_x003A_columnformat=""ShortDateTime"" msprop:dbtable=""feetimeperiod"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""timeperiod"" msdata:Caption=""Time period"" msprop:dbcolumn=""timeperiod"" msprop:expression="""" msprop:dbtable=""feetimeperiod"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""timeperiodbegtime"" msdata:Caption=""Time period beg time"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""timeperiodbegtime"" msprop:expression="""" msprop:dbtable=""feetimeperiod"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""timeperiodendtime"" msdata:Caption=""Time period end time"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""timeperiodendtime"" msprop:expression="""" msprop:dbtable=""feetimeperiod"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""timeperioddaytype"" msdata:Caption=""Time periodday type"" msprop:dbcolumn=""timeperioddaytype"" msprop:expression="""" msprop:dbtable=""feetimeperiod"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""priceevent"" msdata:Caption=""Price event"" msprop:dbcolumn=""priceevent"" msprop:expression="""" msprop:dbtable=""feetimeperiod"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""eventdate"" msdata:Caption=""Event date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""eventdate"" msprop:expression="""" msprop:dbtable=""feetimeperiod"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""timemethod"" msdata:Caption=""Time method"" msprop:dbcolumn=""timemethod"" msprop:expression="""" msprop:dbtable=""feetimeperiod"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""tsperiod"" msdata:Caption=""TS period"" msprop:dbcolumn=""tsperiod"" msprop:expression="""" msprop:dbtable=""feetimeperiod"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""promptoffsetdays"" msdata:Caption=""Prompt offset days"" msprop:dbcolumn=""promptoffsetdays"" msprop:expression="""" msprop:dbtable=""feetimeperiod"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""promptmonth"" msdata:Caption=""Prompt month"" msprop:dbcolumn=""promptmonth"" msprop:expression="""" msprop:dbtable=""feetimeperiod"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""currencytimeperiod"" msdata:Caption=""Currency timeperiod"" msprop:dbcolumn=""currencytimeperiod"" msprop:expression="""" msprop:dbtable=""feetimeperiod"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""rollmonth"" msdata:Caption=""Roll Month"" msprop:dbcolumn=""rollmonth"" msprop:expression="""" msprop:dbtable=""feetimeperiod"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""currencyresolutiontype"" msdata:Caption=""Currency Resolution Type"" msprop:dbcolumn=""currencyresolutiontype"" msprop:expression="""" msprop:dbtable=""feetimeperiod"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""fee_h_version"" msdata:Caption=""Version"" msprop:dbcolumn=""h_version"" msprop:expression="""" msprop:dbtable=""fee"" type=""xs:long"" default=""1"" minOccurs=""0""/>
											<xs:element name=""feeproduct_h_version"" msdata:Caption=""Version"" msprop:dbcolumn=""h_version"" msprop:expression="""" msprop:dbtable=""feeproduct"" type=""xs:long"" default=""1"" minOccurs=""0""/>
											<xs:element name=""feetimeperiod_h_version"" msdata:Caption=""Version"" msprop:dbcolumn=""h_version"" msprop:expression="""" msprop:dbtable=""feetimeperiod"" type=""xs:long"" default=""1"" minOccurs=""0""/>
											<xs:element name=""feesettlement_h_version"" msdata:Caption=""Version"" msprop:dbcolumn=""h_version"" msprop:expression="""" msprop:dbtable=""feesettlement"" type=""xs:long"" default=""1"" minOccurs=""0""/>
											<xs:element name=""feetier_h_version"" msdata:Caption=""Version"" msprop:dbcolumn=""h_version"" msprop:expression="""" msprop:dbtable=""feetier"" type=""xs:long"" default=""1"" minOccurs=""0""/>
											<xs:element name=""dsthandling"" msdata:Caption=""DST Handling"" msprop:dbcolumn=""dsthandling"" msprop:expression="""" msprop:dbtable=""feetimeperiod"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""fixingdate"" msdata:Caption=""Fixing date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""fixingdate"" msprop:expression="""" msprop:dbtable=""fee"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""postdate"" msdata:Caption=""Post date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""postdate"" msprop:expression="""" msprop:pricedetail_x003A_columnformat=""ShortDateTime"" msprop:dbtable=""fee"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""RowNumber"" type=""xs:int"" minOccurs=""0""/>
											<xs:element name=""fee_id"" type=""xs:int"" minOccurs=""0""/>
											<xs:element name=""creationname"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""creationdate"" msdata:DateTimeMode=""Unspecified"" type=""xs:dateTime"" minOccurs=""0""/>
										</xs:sequence>
									</xs:complexType>
								</xs:element>
								<xs:element name=""positioncomponent"" msprop:ProxyVersion=""v2"" msprop:viewpane=""positioncomponent"">
									<xs:complexType>
										<xs:sequence>
											<xs:element name=""position"" msdata:Caption=""Position"" msprop:dbcolumn=""position"" msprop:expression="""" msprop:dbtable=""positioncomponent"" default="""">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""begtime"" msdata:Caption=""Beg time"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""begtime"" msprop:expression="""" msprop:dbtable=""positioncomponent"" type=""xs:dateTime"" default=""2024-11-18T00:00:00""/>
											<xs:element name=""endtime"" msdata:Caption=""End time"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""endtime"" msprop:expression="""" msprop:dbtable=""positioncomponent"" type=""xs:dateTime"" default=""2024-11-18T00:00:00""/>
											<xs:element name=""component"" msdata:Caption=""Component"" msprop:dbcolumn=""component"" msprop:expression="""" msprop:dbtable=""positioncomponent"" default="""">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""24""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""factor"" msdata:Caption=""Factor"" msprop:dbcolumn=""factor"" msprop:expression="""" msprop:dbtable=""positioncomponent"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""creationname"" msdata:Caption=""Creation name"" msprop:dbcolumn=""creationname"" msprop:expression="""" msprop:dbtable=""positioncomponent"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""creationdate"" msdata:Caption=""Creation date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""creationdate"" msprop:expression="""" msprop:dbtable=""positioncomponent"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""revisionname"" msdata:Caption=""Revision name"" msprop:dbcolumn=""revisionname"" msprop:expression="""" msprop:dbtable=""positioncomponent"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""revisiondate"" msdata:Caption=""Revision date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""revisiondate"" msprop:expression="""" msprop:dbtable=""positioncomponent"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""RowNumber"" type=""xs:int"" minOccurs=""0""/>
										</xs:sequence>
									</xs:complexType>
								</xs:element>
								<xs:element name=""optiondetail"" msprop:ProxyVersion=""v2"" msprop:viewpane=""optiondetail"">
									<xs:complexType>
										<xs:sequence>
											<xs:element name=""position"" msdata:Caption=""Position"" msprop:dbcolumn=""position"" msprop:expression="""" msprop:dbtable=""optiondetail"" default="""">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""surrogate"" msdata:AutoIncrement=""true"" msdata:AutoIncrementStep=""-1"" msdata:Caption=""Surrogate"" type=""xs:decimal""/>
											<xs:element name=""positiontype"" msdata:Caption=""Position type"" msprop:dbcolumn=""positiontype"" msprop:expression="""" msprop:dbtable=""optiondetail"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""product"" msdata:Caption=""Product"" msprop:dbcolumn=""product"" msprop:expression="""" msprop:dbtable=""optiondetail"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""24""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""positionmode"" msdata:Caption=""Position mode"" msprop:dbcolumn=""positionmode"" msprop:expression="""" msprop:dbtable=""optiondetail"" default=""PHYSICAL"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""baskettype"" msdata:Caption=""Basket type"" msprop:dbcolumn=""baskettype"" msprop:expression="""" msprop:dbtable=""optiondetail"" default=""BASKET 1"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""price"" msdata:Caption=""Price"" msprop:dbcolumn=""price"" msprop:expression="""" msprop:dbtable=""optiondetail"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""priceunit"" msdata:Caption=""Price unit"" msprop:dbcolumn=""priceunit"" msprop:expression="""" msprop:dbtable=""optiondetail"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""currency"" msdata:Caption=""Currency"" msprop:dbcolumn=""currency"" msprop:expression="""" msprop:dbtable=""optiondetail"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""offsethours"" msdata:Caption=""Offset hours"" msprop:dbcolumn=""offsethours"" msprop:expression="""" msprop:dbtable=""optiondetail"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""quantityfactor"" msdata:Caption=""Quantity factor"" msprop:dbcolumn=""quantityfactor"" msprop:expression="""" msprop:dbtable=""optiondetail"" type=""xs:decimal"" default=""1"" minOccurs=""0""/>
											<xs:element name=""timeunit"" msdata:Caption=""Time unit"" msprop:dbcolumn=""timeunit"" msprop:expression="""" msprop:dbtable=""optiondetail"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""productunit"" msdata:Caption=""Product unit"" msprop:dbcolumn=""productunit"" msprop:expression="""" msprop:dbtable=""optiondetail"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""productcurrency"" msdata:Caption=""Product Currency"" msprop:dbcolumn=""productcurrency"" msprop:expression="""" msprop:dbtable=""optiondetail"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""carrier"" msdata:Caption=""Carrier"" msprop:dbcolumn=""carrier"" msprop:expression="""" msprop:dbtable=""optiondetail"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""location"" msdata:Caption=""Location"" msprop:dbcolumn=""location"" msprop:expression="""" msprop:dbtable=""optiondetail"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""creationname"" msdata:Caption=""Creation Name"" msprop:dbcolumn=""creationname"" msprop:expression="""" msprop:dbtable=""optiondetail"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""creationdate"" msdata:Caption=""Creation Date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""creationdate"" msprop:expression="""" msprop:dbtable=""optiondetail"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""revisionname"" msdata:Caption=""Revision Name"" msprop:dbcolumn=""revisionname"" msprop:expression="""" msprop:dbtable=""optiondetail"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""revisiondate"" msdata:Caption=""Revision Date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""revisiondate"" msprop:expression="""" msprop:dbtable=""optiondetail"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""RowNumber"" type=""xs:int"" minOccurs=""0""/>
										</xs:sequence>
									</xs:complexType>
								</xs:element>
								<xs:element name=""triggerprice"" msprop:ProxyVersion=""v2"">
									<xs:complexType>
										<xs:sequence>
											<xs:element name=""position"" msdata:Caption=""Position"" default="""">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""surrogate"" msdata:AutoIncrement=""true"" msdata:AutoIncrementStep=""-1"" msdata:Caption=""Surrogate"" type=""xs:decimal""/>
											<xs:element name=""triggerdate"" msdata:Caption=""Trigger date"" msdata:DateTimeMode=""Unspecified"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""triggertype"" msdata:Caption=""Trigger type"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""begtime"" msdata:Caption=""Beg time"" msdata:DateTimeMode=""Unspecified"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""endtime"" msdata:Caption=""End time"" msdata:DateTimeMode=""Unspecified"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""triggerquantity"" msdata:Caption=""Trigger quantity"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""triggerprice"" msdata:Caption=""Trigger price"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""meter"" msdata:Caption=""Meter"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""triggerpricestatus"" msdata:Caption=""Price status"" default=""PENDING"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""exchangeindex"" msdata:Caption=""Exchange index"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""timeperiod"" msdata:Caption=""Time period"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""triggerunit"" msdata:Caption=""Trigger unit"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""creationname"" msdata:Caption=""Creation name"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""creationdate"" msdata:Caption=""Creation date"" msdata:DateTimeMode=""Unspecified"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""revisionname"" msdata:Caption=""Revision name"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""revisiondate"" msdata:Caption=""Revision date"" msdata:DateTimeMode=""Unspecified"" type=""xs:dateTime"" minOccurs=""0""/>
										</xs:sequence>
									</xs:complexType>
								</xs:element>
								<xs:element name=""verticalquantitydetail"" msprop:ProxyVersion=""v2"" msprop:viewpane=""verticalquantitydetail"">
									<xs:complexType>
										<xs:sequence>
											<xs:element name=""position"" msdata:Caption=""Position"" msprop:dbcolumn=""position"" msprop:verticalquantitydetail_x003A_sortorder=""ASC"" msprop:expression="""" msprop:verticalquantitydetail_x003A_sortseq=""1"" msprop:dbtable=""position"" default="""">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""posdetail"" msprop:dbcolumn=""posdetail"" msprop:verticalquantitydetail_x003A_sortorder=""ASC"" msprop:expression="""" msprop:verticalquantitydetail_x003A_sortseq=""2"" msprop:dbtable="""" type=""xs:string"" default=""""/>
											<xs:element name=""surrogate"" msdata:AutoIncrement=""true"" msdata:AutoIncrementStep=""-1"" type=""xs:decimal""/>
											<xs:element name=""begtime"" msdata:DateTimeMode=""Unspecified"" msprop:dbtable="""" msprop:verticalquantitydetail_x003A_sortseq=""4"" msprop:dbcolumn=""begtime"" msprop:verticalquantitydetail_x003A_columnformat=""ShortTime"" msprop:expression="""" msprop:verticalquantitydetail_x003A_sortorder=""ASC"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""endtime"" msdata:DateTimeMode=""Unspecified"" msprop:dbtable="""" msprop:verticalquantitydetail_x003A_sortseq=""5"" msprop:dbcolumn=""endtime"" msprop:verticalquantitydetail_x003A_columnformat=""ShortTime"" msprop:expression="""" msprop:verticalquantitydetail_x003A_sortorder=""ASC"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""quantity"" msprop:dbcolumn=""quantity"" msprop:expression="""" msprop:dbtable="""" msprop:verticalquantitydetail_x003A_columnformat=""#,##0.00"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""price"" msprop:dbcolumn="""" msprop:expression="""" msprop:dbtable="""" msprop:verticalquantitydetail_x003A_columnformat=""#,##0.00"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""quantitystatus"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""posstatus"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""daylightsaving"" msprop:dbcolumn=""daylightsaving"" msprop:expression="""" msprop:dbtable="""" msprop:verticalquantitydetail_x003A_columnformat=""Boolean"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""begdate"" msdata:DateTimeMode=""Unspecified"" msprop:dbtable="""" msprop:verticalquantitydetail_x003A_sortseq=""3"" msprop:dbcolumn=""begtime"" msprop:verticalquantitydetail_x003A_columnformat=""ShortDate"" msprop:expression="""" msprop:verticalquantitydetail_x003A_sortorder=""ASC"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""RowNumber"" type=""xs:int"" minOccurs=""0""/>
										</xs:sequence>
									</xs:complexType>
								</xs:element>
								<xs:element name=""pretradecreditexposure"" msprop:ProxyVersion=""v2"" msprop:viewpane=""pretradecreditcheck"">
									<xs:complexType>
										<xs:sequence>
											<xs:element name=""status"" msprop:dbcolumn=""status"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""company"" msprop:dbcolumn=""company"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""creditparty"" msprop:dbcolumn=""creditparty"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""creditcontract"" msprop:dbcolumn=""creditcontract"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""creditproduct"" msprop:dbcolumn=""creditproduct"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""credittrade"" msprop:dbcolumn=""credittrade"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""exposurelimit"" msprop:dbcolumn=""exposurelimit"" msprop:expression="""" msprop:dbtable="""" msprop:pretradecreditcheck_x003A_columnformat=""#,##0;(#,##0)"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""currency"" msprop:dbcolumn=""currency"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""recordedvaluationtime"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""recordedvaluationtime"" msprop:expression="""" msprop:dbtable="""" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""exposure"" msprop:dbcolumn=""exposure"" msprop:expression="""" msprop:dbtable="""" msprop:pretradecreditcheck_x003A_columnformat=""#,##0;(#,##0)"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""collateralsent"" msprop:dbcolumn=""collateralsent"" msprop:expression="""" msprop:dbtable="""" msprop:pretradecreditcheck_x003A_columnformat=""#,##0;(#,##0)"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""collateralheld"" msprop:dbcolumn=""collateralheld"" msprop:expression="""" msprop:dbtable="""" msprop:pretradecreditcheck_x003A_columnformat=""#,##0;(#,##0)"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""collateral"" msprop:dbcolumn=""collateral"" msprop:expression="""" msprop:dbtable="""" msprop:pretradecreditcheck_x003A_columnformat=""#,##0;(#,##0)"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""open"" msprop:dbcolumn=""open"" msprop:expression="""" msprop:dbtable="""" msprop:pretradecreditcheck_x003A_columnformat=""#,##0;(#,##0)"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""current"" msprop:dbcolumn=""current"" msprop:expression="""" msprop:dbtable="""" msprop:pretradecreditcheck_x003A_columnformat=""#,##0;(#,##0)"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""delivered"" msprop:dbcolumn=""delivered"" msprop:expression="""" msprop:dbtable="""" msprop:pretradecreditcheck_x003A_columnformat=""#,##0;(#,##0)"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""forward"" msprop:dbcolumn=""forward"" msprop:expression="""" msprop:dbtable="""" msprop:pretradecreditcheck_x003A_columnformat=""#,##0;(#,##0)"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""prospectivevaluationtime"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""prospectivevaluationtime"" msprop:expression="""" msprop:dbtable="""" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""newtradeopen"" msprop:dbcolumn=""newtradeopen"" msprop:expression="""" msprop:dbtable="""" msprop:pretradecreditcheck_x003A_columnformat=""#,##0;(#,##0)"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""newtradecurrent"" msprop:dbcolumn=""newtradecurrent"" msprop:expression="""" msprop:dbtable="""" msprop:pretradecreditcheck_x003A_columnformat=""#,##0;(#,##0)"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""newtradedelivered"" msprop:dbcolumn=""newtradedelivered"" msprop:expression="""" msprop:dbtable="""" msprop:pretradecreditcheck_x003A_columnformat=""#,##0;(#,##0)"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""newtradecollateral"" msprop:dbcolumn=""newtradecollateral"" msprop:expression="""" msprop:dbtable="""" msprop:pretradecreditcheck_x003A_columnformat=""#,##0;(#,##0)"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""newtradeforward"" msprop:dbcolumn=""newtradeforward"" msprop:expression="""" msprop:dbtable="""" msprop:pretradecreditcheck_x003A_columnformat=""#,##0;(#,##0)"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""newtradecollateralheld"" msprop:dbcolumn=""newtradecollateralheld"" msprop:expression="""" msprop:dbtable="""" msprop:pretradecreditcheck_x003A_columnformat=""#,##0;(#,##0)"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""newtradecollateralsent"" msprop:dbcolumn=""newtradecollateralsent"" msprop:expression="""" msprop:dbtable="""" msprop:pretradecreditcheck_x003A_columnformat=""#,##0;(#,##0)"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""newtradeexposure"" msprop:dbcolumn=""newtradeexposure"" msprop:expression="""" msprop:dbtable="""" msprop:pretradecreditcheck_x003A_columnformat=""#,##0;(#,##0)"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""newtradeavailablecredit"" msprop:dbcolumn=""newtradeavailablecredit"" msprop:expression="""" msprop:dbtable="""" msprop:pretradecreditcheck_x003A_columnformat=""#,##0;(#,##0)"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""availablecredit"" msprop:dbcolumn=""availablecredit"" msprop:expression="""" msprop:dbtable="""" msprop:pretradecreditcheck_x003A_columnformat=""#,##0;(#,##0)"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""counterparty"" msprop:dbcolumn=""counterparty"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""level"" msprop:dbcolumn=""level"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""limitrange"" msprop:dbcolumn=""limitrange"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""validation"" msprop:dbcolumn=""validation"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""pretradecreditexposureid"" msdata:AutoIncrement=""true"" msdata:AutoIncrementSeed=""-1"" msdata:AutoIncrementStep=""-1"" type=""xs:int""/>
											<xs:element name=""RowNumber"" type=""xs:int"" minOccurs=""0""/>
										</xs:sequence>
									</xs:complexType>
								</xs:element>
								<xs:element name=""creditexposure"" msprop:ProxyVersion=""v2"">
									<xs:complexType>
										<xs:sequence>
											<xs:element name=""creditexposuredetail"" msdata:Caption=""Credit exposure detail"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""creditparty"" msdata:Caption=""Creditparty"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""100""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""company"" msdata:Caption=""Company"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""100""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""position"" msdata:Caption=""Position"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""posdetail"" msdata:Caption=""Pos Detail"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""begtime"" msdata:Caption=""Beg time"" msdata:DateTimeMode=""Unspecified"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""endtime"" msdata:Caption=""End time"" msdata:DateTimeMode=""Unspecified"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""value"" msdata:Caption=""Value"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""marketvalue"" msdata:Caption=""Market value"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""creditexposure"" msdata:Caption=""Credit Exposure"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""exposure"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""credittype"" msdata:Caption=""Credit type"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""shipment"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""measure"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""quantitystatus"" msdata:Caption=""Quantity status"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""quantitytype"" msdata:Caption=""Quantity Type"" default=""RECEIPT"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""description"" msdata:Caption=""Description"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""1024""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""quantity"" msdata:Caption=""Quantity"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""pricequantity"" msdata:Caption=""Price quantity"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""price"" msdata:Caption=""Price"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""marketprice"" msdata:Caption=""Market Price"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""currency"" msdata:Caption=""Currency"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""priceunit"" msdata:Caption=""Price unit"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""optionvalue"" msdata:Caption=""Option value"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""contract"" msdata:Caption=""Contract"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""trade"" msdata:Caption=""Trade"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""trader"" msdata:Caption=""Trader"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""tradebook"" msdata:Caption=""Trade book"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""transactiontype"" msdata:Caption=""Transaction type"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""4""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""product"" msdata:Caption=""Product"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""24""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""tradetype"" msdata:Caption=""Trade type"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""carrier"" msdata:Caption=""Carrier"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""location"" msdata:Caption=""Location"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""fee"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""hedge"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""producttype"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""currencyfactor"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""feetype"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""npvfactor"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""valuationtime"" msdata:Caption=""Valuation time"" msdata:DateTimeMode=""Unspecified"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""exposurequantity"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""positionstatus"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""settlementstatus"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""validation"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""volatility"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""var"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""marketarea"" msdata:Caption=""Market area"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""findetail"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""fintransact"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""account"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""strategy"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""component"" msdata:Caption=""Component"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""24""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""invoice"" msdata:Caption=""Invoice"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""invoicedate"" msdata:DateTimeMode=""Unspecified"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""property"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""asset"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""department"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""grossquantity"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""tier"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""decint"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""grossvalue"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""credit"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""debit"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""balance"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""taxlocation"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""confirmstatus"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""subledger"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""acctstatus"" msdata:Caption=""Acct status"" default=""0"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""source"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""lease"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""pipeline"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""point"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""controlarea"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""optionposition"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""tradetimezone"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""optiontype"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""settletimezone"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""futurematch"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""positionmode"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""positiontype"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""settlementdate"" msdata:DateTimeMode=""Unspecified"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""quality"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""valuationmode"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""creditid"" msdata:Caption=""Credit id"" type=""xs:int"" minOccurs=""0""/>
											<xs:element name=""mtmaccrualdate"" msdata:DateTimeMode=""Unspecified"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""exposurelimit"" msdata:Caption=""AR exposure limit"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""settleacctcode"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""collateral"" msdata:Caption=""Collateral"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""reverseexposurelimit"" msdata:AutoIncrementSeed=""-1"" msdata:AutoIncrementStep=""-1"" msdata:Caption=""Ap exposure limit"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""quantitylimit"" msdata:Caption=""AR Exposure Quantity Limit"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""reversequantitylimit"" msdata:Caption=""AP Exposure Quantity Limit"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""direction"" msdata:Caption=""Direction"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""creditproduct"" msdata:Caption=""Credit product"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""24""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""creditcontract"" msdata:Caption=""Credit contract"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""creationname"" msdata:Caption=""Creation name"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""creationdate"" msdata:Caption=""Creation date"" msdata:DateTimeMode=""Unspecified"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""revisionname"" msdata:Caption=""Revision name"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""revisiondate"" msdata:Caption=""Revision date"" msdata:DateTimeMode=""Unspecified"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""reverseexposure"" msdata:Caption=""Reverse Exposure"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""forwardtime"" msdata:DateTimeMode=""Unspecified"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""credittrade"" msdata:Caption=""Credit trade"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""creditexposuretype"" msdata:Caption=""Credit exposure type"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""security"" msdata:Caption=""Security"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""guarantor"" msdata:Caption=""Guarantor"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""positionclass"" msdata:Caption=""Position Class"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""commodityclass"" msdata:Caption=""Commodity Class"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""unnettedexposure"" msdata:Caption=""Unnetted Exposure"" type=""xs:decimal"" default=""0"" minOccurs=""0""/>
											<xs:element name=""duedate"" msdata:Caption=""Duedate"" msdata:DateTimeMode=""Unspecified"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""holidaycalendar"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""memo"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""marginratingmethod"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""warninglevel"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""reversewarninglevel"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""pfecalculationtype"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""creditmode"" msdata:Caption=""Credit mode"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""creditvaluationmode"" msdata:Caption=""Valuation mode"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
										</xs:sequence>
									</xs:complexType>
								</xs:element>
								<xs:element name=""pretradeforwardcreditexposure"" msprop:ProxyVersion=""v2"" msprop:viewpane=""pretradeforwardcreditcheck"">
									<xs:complexType>
										<xs:sequence>
											<xs:element name=""status"" msprop:dbcolumn=""status"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""recordedvaluationtime"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""recordedvaluationtime"" msprop:expression="""" msprop:dbtable="""" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""forwardday"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""forwardday"" msprop:expression="""" msprop:dbtable="""" msprop:pretradeforwardcreditcheck_x003A_columnformat=""dd-MMM-yyyy"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""company"" msprop:dbcolumn=""company"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""creditparty"" msprop:dbcolumn=""creditparty"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""creditcontract"" msprop:dbcolumn=""creditcontract"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""creditproduct"" msprop:dbcolumn=""creditproduct"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""credittrade"" msprop:dbcolumn=""credittrade"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""exposurelimit"" msprop:dbcolumn=""exposurelimit"" msprop:expression="""" msprop:dbtable="""" msprop:pretradeforwardcreditcheck_x003A_columnformat=""#,##0;(#,##0)"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""currency"" msprop:dbcolumn=""currency"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""exposure"" msprop:dbcolumn=""exposure"" msprop:expression="""" msprop:dbtable="""" msprop:pretradeforwardcreditcheck_x003A_columnformat=""#,##0;(#,##0)"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""collateralsent"" msprop:dbcolumn=""collateralsent"" msprop:expression="""" msprop:dbtable="""" msprop:pretradeforwardcreditcheck_x003A_columnformat=""#,##0;(#,##0)"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""collateralheld"" msprop:dbcolumn=""collateralheld"" msprop:expression="""" msprop:dbtable="""" msprop:pretradeforwardcreditcheck_x003A_columnformat=""#,##0;(#,##0)"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""collateral"" msprop:dbcolumn=""collateral"" msprop:expression="""" msprop:dbtable="""" msprop:pretradeforwardcreditcheck_x003A_columnformat=""#,##0;(#,##0)"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""open"" msprop:dbcolumn=""open"" msprop:expression="""" msprop:dbtable="""" msprop:pretradeforwardcreditcheck_x003A_columnformat=""#,##0;(#,##0)"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""current"" msprop:dbcolumn=""current"" msprop:expression="""" msprop:dbtable="""" msprop:pretradeforwardcreditcheck_x003A_columnformat=""#,##0;(#,##0)"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""delivered"" msprop:dbcolumn=""delivered"" msprop:expression="""" msprop:dbtable="""" msprop:pretradeforwardcreditcheck_x003A_columnformat=""#,##0;(#,##0)"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""forward"" msprop:dbcolumn=""forward"" msprop:expression="""" msprop:dbtable="""" msprop:pretradeforwardcreditcheck_x003A_columnformat=""#,##0;(#,##0)"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""newtradeopen"" msprop:dbcolumn=""newtradeopen"" msprop:expression="""" msprop:dbtable="""" msprop:pretradeforwardcreditcheck_x003A_columnformat=""#,##0;(#,##0)"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""newtradecurrent"" msprop:dbcolumn=""newtradecurrent"" msprop:expression="""" msprop:dbtable="""" msprop:pretradeforwardcreditcheck_x003A_columnformat=""#,##0;(#,##0)"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""newtradedelivered"" msprop:dbcolumn=""newtradedelivered"" msprop:expression="""" msprop:dbtable="""" msprop:pretradeforwardcreditcheck_x003A_columnformat=""#,##0;(#,##0)"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""newtradeforward"" msprop:dbcolumn=""newtradeforward"" msprop:expression="""" msprop:dbtable="""" msprop:pretradeforwardcreditcheck_x003A_columnformat=""#,##0;(#,##0)"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""newtradecollateral"" msprop:dbcolumn=""newtradecollateral"" msprop:expression="""" msprop:dbtable="""" msprop:pretradeforwardcreditcheck_x003A_columnformat=""#,##0;(#,##0)"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""newtradecollateralheld"" msprop:dbcolumn=""newtradecollateralheld"" msprop:expression="""" msprop:dbtable="""" msprop:pretradeforwardcreditcheck_x003A_columnformat=""#,##0;(#,##0)"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""newtradecollateralsent"" msprop:dbcolumn=""newtradecollateralsent"" msprop:expression="""" msprop:dbtable="""" msprop:pretradeforwardcreditcheck_x003A_columnformat=""#,##0;(#,##0)"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""newtradeexposure"" msprop:dbcolumn=""newtradeexposure"" msprop:expression="""" msprop:dbtable="""" msprop:pretradeforwardcreditcheck_x003A_columnformat=""#,##0;(#,##0)"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""newtradeavailablecredit"" msprop:dbcolumn=""newtradeavailablecredit"" msprop:expression="""" msprop:dbtable="""" msprop:pretradeforwardcreditcheck_x003A_columnformat=""#,##0;(#,##0)"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""availablecredit"" msprop:dbcolumn=""availablecredit"" msprop:expression="""" msprop:dbtable="""" msprop:pretradeforwardcreditcheck_x003A_columnformat=""#,##0;(#,##0)"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""counterparty"" msprop:dbcolumn=""counterparty"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""level"" msprop:dbcolumn=""level"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""limitrange"" msprop:dbcolumn=""limitrange"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""validation"" msprop:dbcolumn=""validation"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""pretradeforwardcreditexposureid"" msdata:AutoIncrement=""true"" msdata:AutoIncrementSeed=""-1"" msdata:AutoIncrementStep=""-1"" type=""xs:int""/>
											<xs:element name=""RowNumber"" type=""xs:int"" minOccurs=""0""/>
										</xs:sequence>
									</xs:complexType>
								</xs:element>
								<xs:element name=""creditexposurechart"" msprop:ProxyVersion=""v2"" msprop:viewpane=""pretradecreditcheckchart"">
									<xs:complexType>
										<xs:sequence>
											<xs:element name=""credittype"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""existingexposure"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""proposedexposure"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""RowNumber"" type=""xs:int"" minOccurs=""0""/>
										</xs:sequence>
									</xs:complexType>
								</xs:element>
								<xs:element name=""pretradevalidationerror"" msprop:ProxyVersion=""v2"">
									<xs:complexType>
										<xs:sequence>
											<xs:element name=""errormessage"" type=""xs:string"" minOccurs=""0""/>
										</xs:sequence>
									</xs:complexType>
								</xs:element>
								<xs:element name=""pretradecreditcheckmatrix"" msprop:ProxyVersion=""v2"" msprop:viewpane=""pretradecreditcheckmatrix"">
									<xs:complexType>
										<xs:sequence>
											<xs:element name=""colb"" msprop:dbcolumn=""colb"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""col1"" msprop:dbcolumn=""col1"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""col2"" msprop:dbcolumn=""col2"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""col3"" msprop:dbcolumn=""col3"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""col4"" msprop:dbcolumn=""col4"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""col5"" msprop:dbcolumn=""col5"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""col6"" msprop:dbcolumn=""col6"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""col7"" msprop:dbcolumn=""col7"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""pretradecreditcheckmatrixid"" msdata:AutoIncrement=""true"" msdata:AutoIncrementSeed=""-1"" msdata:AutoIncrementStep=""-1"" type=""xs:int""/>
											<xs:element name=""RowNumber"" type=""xs:int"" minOccurs=""0""/>
										</xs:sequence>
									</xs:complexType>
								</xs:element>
								<xs:element name=""pretradeforwardcreditcheckmatrix"" msprop:ProxyVersion=""v2"" msprop:viewpane=""pretradeforwardcreditcheckmatrix"">
									<xs:complexType>
										<xs:sequence>
											<xs:element name=""colb"" msprop:dbcolumn=""colb"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""col1"" msprop:dbcolumn=""col1"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""col2"" msprop:dbcolumn=""col2"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""col3"" msprop:dbcolumn=""col3"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""col4"" msprop:dbcolumn=""col4"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""col5"" msprop:dbcolumn=""col5"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""col6"" msprop:dbcolumn=""col6"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""col7"" msprop:dbcolumn=""col7"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""col8"" msprop:dbcolumn=""col8"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""col9"" msprop:dbcolumn=""col9"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""pretradeforwardcreditcheckmatrixid"" msdata:AutoIncrement=""true"" msdata:AutoIncrementSeed=""-1"" msdata:AutoIncrementStep=""-1"" type=""xs:int""/>
											<xs:element name=""RowNumber"" type=""xs:int"" minOccurs=""0""/>
										</xs:sequence>
									</xs:complexType>
								</xs:element>
								<xs:element name=""trade_approval"" msprop:ActiveRowIndex=""0"" msprop:viewpane=""collaboration_approval"" msprop:dbtable=""approval"" msprop:IsCollaboration=""True"">
									<xs:complexType>
										<xs:sequence>
											<xs:element name=""collaboration"" msdata:Caption=""Collaboration"" msprop:dbcolumn=""collaboration"" msprop:expression="""" msprop:dbtable=""approval"" msprop:approval_x003A_gridseq=""1"" default="""">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""approvaltype"" msdata:Caption=""Approval type"" msprop:dbcolumn=""approvaltype"" msprop:expression="""" msprop:dbtable=""approval"" msprop:approval_x003A_gridseq=""2"" default="""">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""dbtable"" msdata:Caption=""DB table"" msprop:dbcolumn=""dbtable"" msprop:expression="""" msprop:dbtable=""approval"" msprop:approval_x003A_gridseq=""3"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""approval"" msdata:Caption=""Approval"" msprop:dbcolumn=""approval"" msprop:expression="""" msprop:dbtable=""approval"" msprop:approval_x003A_gridseq=""4"" type=""xs:boolean"" default=""true"" minOccurs=""0""/>
											<xs:element name=""approvalname"" msdata:Caption=""Approval name"" msprop:dbcolumn=""approvalname"" msprop:expression="""" msprop:dbtable=""approval"" msprop:approval_x003A_gridseq=""5"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""approvaldate"" msdata:Caption=""Approval date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""approvaldate"" msprop:expression="""" msprop:dbtable=""approval"" msprop:approval_x003A_gridseq=""6"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""creationname"" msdata:Caption=""Creation name"" msprop:dbcolumn=""creationname"" msprop:expression="""" msprop:dbtable=""approval"" msprop:approval_x003A_gridseq=""91"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""creationdate"" msdata:Caption=""Creation date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""creationdate"" msprop:expression="""" msprop:dbtable=""approval"" msprop:approval_x003A_gridseq=""92"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""revisionname"" msdata:Caption=""Revision name"" msprop:dbcolumn=""revisionname"" msprop:expression="""" msprop:dbtable=""approval"" msprop:approval_x003A_gridseq=""93"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""revisiondate"" msdata:Caption=""Revision date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""revisiondate"" msprop:expression="""" msprop:dbtable=""approval"" msprop:approval_x003A_gridseq=""94"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""approvallevel"" msdata:Caption=""Approval Level"" msprop:dbcolumn=""approvallevel"" msprop:expression="""" msprop:dbtable=""approvaltype"" type=""xs:int"" default=""0"" minOccurs=""0""/>
											<xs:element name=""RowNumber"" type=""xs:int"" minOccurs=""0""/>
										</xs:sequence>
									</xs:complexType>
								</xs:element>
								<xs:element name=""trade_calendar"" msprop:viewpane=""collaboration_calendar"" msprop:dbtable=""calendar"" msprop:IsCollaboration=""True"">
									<xs:complexType>
										<xs:sequence>
											<xs:element name=""collaboration"" msdata:Caption=""Collaboration"" msprop:dbcolumn=""collaboration"" msprop:expression="""" msprop:dbtable=""calendar"" msprop:calendar_x003A_gridseq=""1"" default="""">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""surrogate"" msdata:AutoIncrement=""true"" msdata:AutoIncrementStep=""-1"" msdata:Caption=""Surrogate"" msprop:dbcolumn=""surrogate"" msprop:expression="""" msprop:dbtable=""calendar"" msprop:calendar_x003A_gridseq=""2"" type=""xs:decimal""/>
											<xs:element name=""calendardate"" msdata:Caption=""Calendar date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""calendardate"" msprop:expression="""" msprop:dbtable=""calendar"" msprop:calendar_x003A_gridseq=""3"" type=""xs:dateTime"" default=""2024-11-18T00:00:00"" minOccurs=""0""/>
											<xs:element name=""calendarevent"" msdata:Caption=""Calendar event"" msprop:dbcolumn=""calendarevent"" msprop:expression="""" msprop:dbtable=""calendar"" msprop:calendar_x003A_gridseq=""4"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""dbtable"" msdata:Caption=""DB table"" msprop:dbcolumn=""dbtable"" msprop:expression="""" msprop:dbtable=""calendar"" msprop:calendar_x003A_gridseq=""5"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""subject"" msprop:dbcolumn=""subject"" msprop:expression="""" msprop:dbtable=""calendar"" msprop:calendar_x003A_gridseq=""6"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""description"" msdata:Caption=""Description"" msprop:dbcolumn=""description"" msprop:expression="""" msprop:dbtable=""calendar"" msprop:calendar_x003A_gridseq=""7"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""2147483647""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""actualdate"" msdata:Caption=""Actual date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""actualdate"" msprop:expression="""" msprop:dbtable=""calendar"" msprop:calendar_x003A_gridseq=""8"" type=""xs:dateTime"" default=""2024-11-18T00:00:00"" minOccurs=""0""/>
											<xs:element name=""contact"" msdata:Caption=""Contact"" msprop:dbcolumn=""contact"" msprop:expression="""" msprop:dbtable=""calendar"" msprop:calendar_x003A_gridseq=""9"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""location"" msdata:Caption=""Location"" msprop:dbcolumn=""location"" msprop:expression="""" msprop:dbtable=""calendar"" msprop:calendar_x003A_gridseq=""10"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""calendarstatus"" msdata:Caption=""Calendar status"" msprop:dbcolumn=""calendarstatus"" msprop:expression="""" msprop:dbtable=""calendar"" msprop:calendar_x003A_gridseq=""11"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""completion"" msdata:Caption=""Completion"" msprop:dbcolumn=""completion"" msprop:expression="""" msprop:dbtable=""calendar"" msprop:calendar_x003A_gridseq=""12"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""completiondate"" msdata:Caption=""Completion date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""completiondate"" msprop:expression="""" msprop:dbtable=""calendar"" msprop:calendar_x003A_gridseq=""13"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""calendartype"" msdata:Caption=""Calendar type"" msprop:dbcolumn=""calendartype"" msprop:expression="""" msprop:dbtable=""calendar"" msprop:calendar_x003A_gridseq=""16"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""calendarpriority"" msdata:Caption=""Calendar priority"" msprop:dbcolumn=""calendarpriority"" msprop:expression="""" msprop:dbtable=""calendar"" msprop:calendar_x003A_gridseq=""18"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""position"" msdata:Caption=""Position"" msprop:dbcolumn=""position"" msprop:expression="""" msprop:dbtable=""calendar"" msprop:calendar_x003A_gridseq=""21"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""measure"" msdata:Caption=""Measure"" msprop:dbcolumn=""measure"" msprop:expression="""" msprop:dbtable=""calendar"" msprop:calendar_x003A_gridseq=""23"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""begtime"" msdata:Caption=""Beg time"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""begtime"" msprop:expression="""" msprop:dbtable=""calendar"" msprop:calendar_x003A_gridseq=""24"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""endtime"" msdata:Caption=""End time"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""endtime"" msprop:expression="""" msprop:dbtable=""calendar"" msprop:calendar_x003A_gridseq=""25"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""timeon"" msdata:Caption=""Time on"" msprop:dbcolumn=""timeon"" msprop:expression="""" msprop:dbtable=""calendar"" msprop:calendar_x003A_gridseq=""26"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""timeoff"" msdata:Caption=""Time off"" msprop:dbcolumn=""timeoff"" msprop:expression="""" msprop:dbtable=""calendar"" msprop:calendar_x003A_gridseq=""27"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""factor"" msdata:Caption=""Factor"" msprop:dbcolumn=""factor"" msprop:expression="""" msprop:dbtable=""calendar"" msprop:calendar_x003A_gridseq=""28"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""hours"" msdata:Caption=""Hours"" msprop:dbcolumn=""hours"" msprop:expression="""" msprop:dbtable=""calendar"" msprop:calendar_x003A_gridseq=""29"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""creationname"" msdata:Caption=""Creation name"" msprop:dbcolumn=""creationname"" msprop:expression="""" msprop:dbtable=""calendar"" msprop:calendar_x003A_gridseq=""91"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""creationdate"" msdata:Caption=""Creation date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""creationdate"" msprop:expression="""" msprop:dbtable=""calendar"" msprop:calendar_x003A_gridseq=""92"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""revisionname"" msdata:Caption=""Revision name"" msprop:dbcolumn=""revisionname"" msprop:expression="""" msprop:dbtable=""calendar"" msprop:calendar_x003A_gridseq=""93"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""revisiondate"" msdata:Caption=""Revision date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""revisiondate"" msprop:expression="""" msprop:dbtable=""calendar"" msprop:calendar_x003A_gridseq=""94"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""appointmentdata"" msdata:Caption=""Appointment data"" msprop:dbcolumn=""appointmentdata"" msprop:expression="""" msprop:dbtable=""calendar"" msprop:calendar_x003A_gridseq=""30"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""RowNumber"" type=""xs:int"" minOccurs=""0""/>
										</xs:sequence>
									</xs:complexType>
								</xs:element>
								<xs:element name=""trade_correspondence"" msprop:viewpane=""collaboration_correspondence"" msprop:dbtable=""correspondence"" msprop:IsCollaboration=""True"">
									<xs:complexType>
										<xs:sequence>
											<xs:element name=""collaboration"" msdata:Caption=""Collaboration"" msprop:dbcolumn=""collaboration"" msprop:expression="""" msprop:dbtable=""correspondence"" msprop:correspondence_x003A_gridseq=""1"" default="""">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""correspondencedate"" msdata:Caption=""Correspondence date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""correspondencedate"" msprop:expression="""" msprop:dbtable=""correspondence"" msprop:correspondence_x003A_gridseq=""2"" type=""xs:dateTime"" default=""2024-11-18T00:00:00""/>
											<xs:element name=""dbtable"" msdata:Caption=""DB table"" msprop:dbcolumn=""dbtable"" msprop:expression="""" msprop:dbtable=""correspondence"" msprop:correspondence_x003A_gridseq=""3"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""correspondencetype"" msdata:Caption=""Correspondence type"" msprop:dbcolumn=""correspondencetype"" msprop:expression="""" msprop:dbtable=""correspondence"" msprop:correspondence_x003A_gridseq=""4"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""counterparty"" msdata:Caption=""Counterparty"" msprop:dbcolumn=""counterparty"" msprop:expression="""" msprop:dbtable=""correspondence"" msprop:correspondence_x003A_gridseq=""5"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""originator"" msdata:Caption=""Originator"" msprop:dbcolumn=""originator"" msprop:expression="""" msprop:dbtable=""correspondence"" msprop:correspondence_x003A_gridseq=""6"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""recipient"" msdata:Caption=""Recipient"" msprop:dbcolumn=""recipient"" msprop:expression="""" msprop:dbtable=""correspondence"" msprop:correspondence_x003A_gridseq=""7"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""subject"" msdata:Caption=""Subject"" msprop:dbcolumn=""subject"" msprop:expression="""" msprop:dbtable=""correspondence"" msprop:correspondence_x003A_gridseq=""8"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""content"" msdata:Caption=""Content"" msprop:dbcolumn=""content"" msprop:expression="""" msprop:dbtable=""correspondence"" msprop:correspondence_x003A_gridseq=""9"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""2147483647""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""creationname"" msdata:Caption=""Creation name"" msprop:dbcolumn=""creationname"" msprop:expression="""" msprop:dbtable=""correspondence"" msprop:correspondence_x003A_gridseq=""91"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""creationdate"" msdata:Caption=""Creation date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""creationdate"" msprop:expression="""" msprop:dbtable=""correspondence"" msprop:correspondence_x003A_gridseq=""92"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""revisionname"" msdata:Caption=""Revision name"" msprop:dbcolumn=""revisionname"" msprop:expression="""" msprop:dbtable=""correspondence"" msprop:correspondence_x003A_gridseq=""93"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""revisiondate"" msdata:Caption=""Revision date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""revisiondate"" msprop:expression="""" msprop:dbtable=""correspondence"" msprop:correspondence_x003A_gridseq=""94"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""RowNumber"" type=""xs:int"" minOccurs=""0""/>
										</xs:sequence>
									</xs:complexType>
								</xs:element>
								<xs:element name=""trade_document"" msprop:viewpane=""collaboration_document"" msprop:dbtable=""document"" msprop:IsCollaboration=""True"">
									<xs:complexType>
										<xs:sequence>
											<xs:element name=""collaboration"" msdata:Caption=""Collaboration"" msprop:dbcolumn=""collaboration"" msprop:expression="""" msprop:dbtable=""document"" msprop:document_x003A_gridseq=""1"" default="""">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""surrogate"" msdata:AutoIncrement=""true"" msdata:AutoIncrementStep=""-1"" msdata:Caption=""Surrogate"" msprop:dbcolumn=""surrogate"" msprop:expression="""" msprop:dbtable=""document"" msprop:document_x003A_gridseq=""2"" type=""xs:decimal""/>
											<xs:element name=""documentdate"" msdata:Caption=""Document date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""documentdate"" msprop:expression="""" msprop:dbtable=""document"" msprop:document_x003A_gridseq=""3"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""dbtable"" msdata:Caption=""DB table"" msprop:dbcolumn=""dbtable"" msprop:expression="""" msprop:dbtable=""document"" msprop:document_x003A_gridseq=""4"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""documenttype"" msdata:Caption=""Document type"" msprop:dbcolumn=""documenttype"" msprop:expression="""" msprop:dbtable=""document"" msprop:document_x003A_gridseq=""5"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""documentformat"" msdata:Caption=""Document format"" msprop:dbcolumn=""documentformat"" msprop:expression="""" msprop:dbtable=""document"" msprop:document_x003A_gridseq=""6"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""documentstatus"" msdata:Caption=""Document status"" msprop:dbcolumn=""documentstatus"" msprop:expression="""" msprop:dbtable=""document"" msprop:document_x003A_gridseq=""7"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""description"" msdata:Caption=""Description"" msprop:dbcolumn=""description"" msprop:expression="""" msprop:dbtable=""document"" msprop:document_x003A_gridseq=""8"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""filename"" msdata:Caption=""File name"" msprop:dbcolumn=""filename"" msprop:expression="""" msprop:dbtable=""document"" msprop:document_x003A_gridseq=""9"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""1024""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""printdate"" msdata:Caption=""Print date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""printdate"" msprop:expression="""" msprop:dbtable=""document"" msprop:document_x003A_gridseq=""10"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""required"" msdata:Caption=""Required"" msprop:dbcolumn=""required"" msprop:expression="""" msprop:dbtable=""document"" msprop:document_x003A_gridseq=""11"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""confirmation"" msdata:Caption=""Confirmation"" msprop:dbcolumn=""confirmation"" msprop:expression="""" msprop:dbtable=""document"" msprop:document_x003A_gridseq=""12"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""confirmdate"" msdata:Caption=""Confirm date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""confirmdate"" msprop:expression="""" msprop:dbtable=""document"" msprop:document_x003A_gridseq=""13"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""email"" msdata:Caption=""Email"" msprop:dbcolumn=""email"" msprop:expression="""" msprop:dbtable=""document"" msprop:document_x003A_gridseq=""15"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""1024""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""emaildate"" msdata:Caption=""Email Date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""emaildate"" msprop:expression="""" msprop:dbtable=""document"" msprop:document_x003A_gridseq=""17"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""faxphone"" msdata:Caption=""Fax"" msprop:dbcolumn=""faxphone"" msprop:expression="""" msprop:dbtable=""document"" msprop:document_x003A_gridseq=""16"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""1024""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""faxdate"" msdata:Caption=""Fax Date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""faxdate"" msprop:expression="""" msprop:dbtable=""document"" msprop:document_x003A_gridseq=""18"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""creationname"" msdata:Caption=""Creation name"" msprop:dbcolumn=""creationname"" msprop:expression="""" msprop:dbtable=""document"" msprop:document_x003A_gridseq=""91"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""creationdate"" msdata:Caption=""Creation date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""creationdate"" msprop:expression="""" msprop:dbtable=""document"" msprop:document_x003A_gridseq=""92"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""revisionname"" msdata:Caption=""Revision name"" msprop:dbcolumn=""revisionname"" msprop:expression="""" msprop:dbtable=""document"" msprop:document_x003A_gridseq=""93"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""revisiondate"" msdata:Caption=""Revision date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""revisiondate"" msprop:expression="""" msprop:dbtable=""document"" msprop:document_x003A_gridseq=""94"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""RowNumber"" type=""xs:int"" minOccurs=""0""/>
										</xs:sequence>
									</xs:complexType>
								</xs:element>
								<xs:element name=""trade_interface"" msprop:dbtable=""interface"" msprop:Generator_RowEvHandlerName=""interfaceRowChangeEventHandler"" msprop:IsCollaboration=""True"" msprop:Generator_RowEvArgName=""interfaceRowChangeEvent"" msprop:viewpane=""collaboration_interface"" msprop:Generator_UserTableName=""interface"" msprop:Generator_TableClassName=""interfaceDataTable"" msprop:Generator_RowClassName=""interfaceRow"" msprop:Generator_TableVarName=""tableinterface"">
									<xs:complexType>
										<xs:sequence>
											<xs:element name=""collaboration"" msdata:Caption=""Collaboration"" msprop:dbcolumn=""collaboration"" msprop:expression="""" msprop:dbtable=""interface"" msprop:interface_x003A_gridseq=""1"" default="""">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""interfaceclass"" msdata:Caption=""Interface class"" msprop:dbcolumn=""interfaceclass"" msprop:expression="""" msprop:dbtable=""interface"" msprop:interface_x003A_gridseq=""2"" default="""">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""dbtable"" msdata:Caption=""Db table"" msprop:dbcolumn=""dbtable"" msprop:expression="""" msprop:dbtable=""interface"" msprop:interface_x003A_gridseq=""3"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""interface"" msdata:Caption=""Interface"" msprop:dbtable=""interface"" msprop:Generator_ColumnPropNameInTable=""interfaceColumn"" msprop:Generator_ColumnVarNameInTable=""columninterface"" msprop:expression="""" msprop:Generator_UserColumnName=""interface"" msprop:dbcolumn=""interface"" msprop:interface_x003A_gridseq=""4"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""creationname"" msdata:Caption=""Creation name"" msprop:dbcolumn=""creationname"" msprop:expression="""" msprop:dbtable=""interface"" msprop:interface_x003A_gridseq=""91"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""creationdate"" msdata:Caption=""Creation date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""creationdate"" msprop:expression="""" msprop:dbtable=""interface"" msprop:interface_x003A_gridseq=""92"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""revisionname"" msdata:Caption=""Revision name"" msprop:dbcolumn=""revisionname"" msprop:expression="""" msprop:dbtable=""interface"" msprop:interface_x003A_gridseq=""93"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""revisiondate"" msdata:Caption=""Revision date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""revisiondate"" msprop:expression="""" msprop:dbtable=""interface"" msprop:interface_x003A_gridseq=""94"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""RowNumber"" type=""xs:int"" minOccurs=""0""/>
										</xs:sequence>
									</xs:complexType>
								</xs:element>
								<xs:element name=""trade_remark"" msprop:viewpane=""collaboration_remark"" msprop:dbtable=""remark"" msprop:IsCollaboration=""True"">
									<xs:complexType>
										<xs:sequence>
											<xs:element name=""collaboration"" msdata:Caption=""Collaboration"" msprop:dbcolumn=""collaboration"" msprop:expression="""" msprop:dbtable=""remark"" msprop:remark_x003A_gridseq=""1"" default="""">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""surrogate"" msdata:AutoIncrement=""true"" msdata:AutoIncrementStep=""-1"" msdata:Caption=""Surrogate"" msprop:remark_x003A_gridseq=""2"" type=""xs:decimal""/>
											<xs:element name=""remarkdate"" msdata:Caption=""Remark date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""remarkdate"" msprop:expression="""" msprop:dbtable=""remark"" msprop:remark_x003A_gridseq=""3"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""dbtable"" msdata:Caption=""DB table"" msprop:dbcolumn=""dbtable"" msprop:expression="""" msprop:dbtable=""remark"" msprop:remark_x003A_gridseq=""4"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""subject"" msprop:dbcolumn=""subject"" msprop:expression="""" msprop:dbtable=""remark"" msprop:remark_x003A_gridseq=""5"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""remarktext"" msdata:Caption=""Remark text"" msprop:dbcolumn=""remarktext"" msprop:expression="""" msprop:dbtable=""remark"" msprop:remark_x003A_gridseq=""6"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""2147483647""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""remarkclass"" msdata:Caption=""Remark Class"" msprop:dbcolumn=""remarkclass"" msprop:expression="""" msprop:dbtable=""remark"" msprop:remark_x003A_gridseq=""7"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""creationname"" msdata:Caption=""Creation name"" msprop:dbcolumn=""creationname"" msprop:expression="""" msprop:dbtable=""remark"" msprop:remark_x003A_gridseq=""91"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""creationdate"" msdata:Caption=""Creation date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""creationdate"" msprop:expression="""" msprop:dbtable=""remark"" msprop:remark_x003A_gridseq=""92"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""revisionname"" msdata:Caption=""Revision name"" msprop:dbcolumn=""revisionname"" msprop:expression="""" msprop:dbtable=""remark"" msprop:remark_x003A_gridseq=""93"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""revisiondate"" msdata:Caption=""Revision date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""revisiondate"" msprop:expression="""" msprop:dbtable=""remark"" msprop:remark_x003A_gridseq=""94"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""RowNumber"" type=""xs:int"" minOccurs=""0""/>
										</xs:sequence>
									</xs:complexType>
								</xs:element>
								<xs:element name=""trade_workflowstate"" msprop:RecursiveChildColumn=""workflowid"" msprop:viewpane=""collaboration_workflowstate"" msprop:RecursiveColumn=""alternateflow"" msprop:dbtable=""workflowstate"" msprop:IsCollaboration=""True"">
									<xs:complexType>
										<xs:sequence>
											<xs:element name=""collaboration"" msdata:Caption=""Collaboration"" msprop:dbcolumn=""collaboration"" msprop:expression="""" msprop:workflowstate_x003A_gridseq=""1"" msprop:dbtable=""workflowstate"" default="""">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""8""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""taskid"" msdata:Caption=""Task"" msprop:dbcolumn=""taskid"" msprop:expression="""" msprop:workflowstate_x003A_gridseq=""2"" msprop:dbtable=""workflowstate"" default="""">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""dbtable"" msdata:Caption=""DB table"" msprop:dbcolumn=""dbtable"" msprop:expression="""" msprop:workflowstate_x003A_gridseq=""6"" msprop:dbtable=""workflowstate"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""dbvalue"" msdata:Caption=""DB value"" msprop:dbcolumn=""dbvalue"" msprop:expression="""" msprop:workflowstate_x003A_gridseq=""7"" msprop:dbtable=""workflowstate"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""32""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""status"" msdata:Caption=""Status"" msprop:dbcolumn=""status"" msprop:expression="""" msprop:workflowstate_x003A_gridseq=""8"" msprop:dbtable=""workflowstate"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""startdate"" msdata:Caption=""Start date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""startdate"" msprop:expression="""" msprop:workflowstate_x003A_gridseq=""9"" msprop:dbtable=""workflowstate"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""completiondate"" msdata:Caption=""Completion date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""completiondate"" msprop:expression="""" msprop:workflowstate_x003A_gridseq=""11"" msprop:dbtable=""workflowstate"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""completedby"" msdata:Caption=""Completed by"" msprop:dbcolumn=""completedby"" msprop:expression="""" msprop:workflowstate_x003A_gridseq=""12"" msprop:dbtable=""workflowstate"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""completed"" msdata:Caption=""Completed"" msprop:dbcolumn=""completed"" msprop:expression="""" msprop:workflowstate_x003A_gridseq=""10"" msprop:dbtable=""workflowstate"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""movetoalternate"" msdata:Caption=""Move to alternate"" msprop:dbcolumn=""movetoalternate"" msprop:expression="""" msprop:workflowstate_x003A_gridseq=""13"" msprop:dbtable=""workflowstate"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""workflowid"" msdata:Caption=""Workflow ID"" msprop:dbcolumn=""workflowid"" msprop:expression="""" msprop:dbtable=""workflowtask"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""seq"" msdata:Caption=""Sequence"" msprop:dbcolumn=""seq"" msprop:expression="""" msprop:dbtable=""workflowtask"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""description"" msdata:Caption=""Description"" msprop:dbcolumn=""description"" msprop:expression="""" msprop:dbtable=""workflowtask"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""alternateflow"" msdata:Caption=""Alternate workflow"" msprop:dbcolumn=""alternateflow"" msprop:expression="""" msprop:dbtable=""workflowtask"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""tasktype"" msdata:Caption=""Task type"" msprop:dbcolumn=""tasktype"" msprop:expression="""" msprop:dbtable=""workflowtask"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""16""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""creationname"" msdata:Caption=""Creation name"" msprop:dbcolumn=""creationname"" msprop:expression="""" msprop:workflowstate_x003A_gridseq=""88"" msprop:dbtable=""workflowstate"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""creationdate"" msdata:Caption=""Creation date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""creationdate"" msprop:expression="""" msprop:workflowstate_x003A_gridseq=""89"" msprop:dbtable=""workflowstate"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""revisionname"" msdata:Caption=""Revision name"" msprop:dbcolumn=""revisionname"" msprop:expression="""" msprop:workflowstate_x003A_gridseq=""90"" msprop:dbtable=""workflowstate"" minOccurs=""0"">
												<xs:simpleType>
													<xs:restriction base=""xs:string"">
														<xs:maxLength value=""64""/>
													</xs:restriction>
												</xs:simpleType>
											</xs:element>
											<xs:element name=""revisiondate"" msdata:Caption=""Revision date"" msdata:DateTimeMode=""Unspecified"" msprop:dbcolumn=""revisiondate"" msprop:expression="""" msprop:workflowstate_x003A_gridseq=""91"" msprop:dbtable=""workflowstate"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""movetogrid"" msdata:Caption=""Move to gridqueue"" msprop:dbcolumn=""movetogrid"" msprop:expression="""" msprop:workflowstate_x003A_gridseq=""14"" msprop:dbtable=""workflowstate"" type=""xs:boolean"" default=""false"" minOccurs=""0""/>
											<xs:element name=""RowNumber"" type=""xs:int"" minOccurs=""0""/>
										</xs:sequence>
									</xs:complexType>
								</xs:element>
								<xs:element name=""summary_band0_summary"" msprop:ProxyVersion=""v2"" msprop:viewpane=""summary"">
									<xs:complexType>
										<xs:sequence>
											<xs:element name=""corporate"" msprop:dbcolumn=""corporate"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""creditparty"" msprop:dbtable="""" msprop:summary_x003A_drill=""True"" msprop:summary_x003A_sortseq=""1"" msprop:summary_x003A_sortorder=""ASC"" msprop:expression="""" msprop:dbcolumn=""creditparty"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""description"" msprop:dbcolumn=""description"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""limit"" msprop:summary_x003A_subtotal=""True"" msprop:dbcolumn=""limit"" msprop:summary_x003A_columnformat=""#,##0;(#,##0)"" msprop:expression="""" msprop:dbtable="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""collateral"" msprop:summary_x003A_subtotal=""True"" msprop:dbcolumn=""collateral"" msprop:summary_x003A_columnformat=""#,##0;(#,##0)"" msprop:expression="""" msprop:dbtable="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""openinvoice"" msprop:summary_x003A_subtotal=""True"" msprop:dbcolumn=""openinvoice"" msprop:summary_x003A_columnformat=""#,##0;(#,##0)"" msprop:expression="""" msprop:dbtable="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""current"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""forward"" msprop:summary_x003A_subtotal=""True"" msprop:dbcolumn=""forward"" msprop:summary_x003A_columnformat=""#,##0;(#,##0)"" msprop:expression="""" msprop:dbtable="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""exposure"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""contract"" msprop:dbtable="""" msprop:summary_x003A_drill=""True"" msprop:summary_x003A_sortseq=""2"" msprop:summary_x003A_sortorder=""ASC"" msprop:expression="""" msprop:dbcolumn=""contract"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""credittype"" msprop:dbtable="""" msprop:summary_x003A_drill=""True"" msprop:summary_x003A_sortseq=""3"" msprop:summary_x003A_sortorder=""ASC"" msprop:expression="""" msprop:dbcolumn=""credittype"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""currentexposure"" msprop:summary_x003A_subtotal=""True"" msprop:dbcolumn=""currentexposure"" msprop:summary_x003A_columnformat=""#,##0;(#,##0)"" msprop:expression="""" msprop:dbtable="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""totalexposure"" msprop:summary_x003A_subtotal=""True"" msprop:dbcolumn=""totalexposure"" msprop:summary_x003A_columnformat=""#,##0;(#,##0)"" msprop:expression="""" msprop:dbtable="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""currency"" msprop:dbcolumn=""currency"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""begtime"" msdata:DateTimeMode=""Unspecified"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""endtime"" msdata:DateTimeMode=""Unspecified"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""company"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""m0"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""groupdesc"" msprop:dbcolumn=""groupdesc"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""groupstatus"" type=""xs:int"" minOccurs=""0""/>
											<xs:element name=""grouplevel"" type=""xs:int"" minOccurs=""0""/>
											<xs:element name=""grouptotal"" type=""xs:int"" minOccurs=""0""/>
											<xs:element name=""counterparty"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""creditdetailreference"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""creditexposure"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""deliveredexposure"" msprop:summary_x003A_subtotal=""True"" msprop:dbcolumn="""" msprop:summary_x003A_columnformat=""#,##0;(#,##0)"" msprop:expression="""" msprop:dbtable="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""availablecredit"" msprop:summary_x003A_subtotal=""True"" msprop:dbcolumn="""" msprop:summary_x003A_columnformat=""#,##0;(#,##0)"" msprop:expression="""" msprop:dbtable="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""m"" msprop:summary_x003A_subtotal=""True"" msprop:dbcolumn=""m"" msprop:summary_x003A_columnformat=""#,##0;(#,##0)"" msprop:expression="""" msprop:dbtable="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""currencyfactor"" msdata:Caption=""Currency factor"" msprop:dbcolumn=""currencyfactor"" msprop:summary_x003A_columnformat=""#,##0"" msprop:expression="""" msprop:dbtable=""valuationdetail"" type=""xs:decimal"" default=""1"" minOccurs=""0""/>
											<xs:element name=""RowNumber"" type=""xs:int"" minOccurs=""0""/>
											<xs:element name=""grouplevel0_summary"" type=""xs:string""/>
											<xs:element name=""grouplevel1_summary"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""grouplevel2_summary"" type=""xs:string"" minOccurs=""0""/>
										</xs:sequence>
									</xs:complexType>
								</xs:element>
								<xs:element name=""summary_band1_summary"" msprop:ProxyVersion=""v2"" msprop:viewpane=""summary"">
									<xs:complexType>
										<xs:sequence>
											<xs:element name=""corporate"" msprop:dbcolumn=""corporate"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""creditparty"" msprop:dbtable="""" msprop:summary_x003A_drill=""True"" msprop:summary_x003A_sortseq=""1"" msprop:summary_x003A_sortorder=""ASC"" msprop:expression="""" msprop:dbcolumn=""creditparty"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""description"" msprop:dbcolumn=""description"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""limit"" msprop:summary_x003A_subtotal=""True"" msprop:dbcolumn=""limit"" msprop:summary_x003A_columnformat=""#,##0;(#,##0)"" msprop:expression="""" msprop:dbtable="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""collateral"" msprop:summary_x003A_subtotal=""True"" msprop:dbcolumn=""collateral"" msprop:summary_x003A_columnformat=""#,##0;(#,##0)"" msprop:expression="""" msprop:dbtable="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""openinvoice"" msprop:summary_x003A_subtotal=""True"" msprop:dbcolumn=""openinvoice"" msprop:summary_x003A_columnformat=""#,##0;(#,##0)"" msprop:expression="""" msprop:dbtable="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""current"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""forward"" msprop:summary_x003A_subtotal=""True"" msprop:dbcolumn=""forward"" msprop:summary_x003A_columnformat=""#,##0;(#,##0)"" msprop:expression="""" msprop:dbtable="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""exposure"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""contract"" msprop:dbtable="""" msprop:summary_x003A_drill=""True"" msprop:summary_x003A_sortseq=""2"" msprop:summary_x003A_sortorder=""ASC"" msprop:expression="""" msprop:dbcolumn=""contract"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""credittype"" msprop:dbtable="""" msprop:summary_x003A_drill=""True"" msprop:summary_x003A_sortseq=""3"" msprop:summary_x003A_sortorder=""ASC"" msprop:expression="""" msprop:dbcolumn=""credittype"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""currentexposure"" msprop:summary_x003A_subtotal=""True"" msprop:dbcolumn=""currentexposure"" msprop:summary_x003A_columnformat=""#,##0;(#,##0)"" msprop:expression="""" msprop:dbtable="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""totalexposure"" msprop:summary_x003A_subtotal=""True"" msprop:dbcolumn=""totalexposure"" msprop:summary_x003A_columnformat=""#,##0;(#,##0)"" msprop:expression="""" msprop:dbtable="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""currency"" msprop:dbcolumn=""currency"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""begtime"" msdata:DateTimeMode=""Unspecified"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""endtime"" msdata:DateTimeMode=""Unspecified"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""company"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""m0"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""groupdesc"" msprop:dbcolumn=""groupdesc"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""groupstatus"" type=""xs:int"" minOccurs=""0""/>
											<xs:element name=""grouplevel"" type=""xs:int"" minOccurs=""0""/>
											<xs:element name=""grouptotal"" type=""xs:int"" minOccurs=""0""/>
											<xs:element name=""counterparty"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""creditdetailreference"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""creditexposure"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""deliveredexposure"" msprop:summary_x003A_subtotal=""True"" msprop:dbcolumn="""" msprop:summary_x003A_columnformat=""#,##0;(#,##0)"" msprop:expression="""" msprop:dbtable="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""availablecredit"" msprop:summary_x003A_subtotal=""True"" msprop:dbcolumn="""" msprop:summary_x003A_columnformat=""#,##0;(#,##0)"" msprop:expression="""" msprop:dbtable="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""m"" msprop:summary_x003A_subtotal=""True"" msprop:dbcolumn=""m"" msprop:summary_x003A_columnformat=""#,##0;(#,##0)"" msprop:expression="""" msprop:dbtable="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""currencyfactor"" msdata:Caption=""Currency factor"" msprop:dbcolumn=""currencyfactor"" msprop:summary_x003A_columnformat=""#,##0"" msprop:expression="""" msprop:dbtable=""valuationdetail"" type=""xs:decimal"" default=""1"" minOccurs=""0""/>
											<xs:element name=""RowNumber"" type=""xs:int"" minOccurs=""0""/>
											<xs:element name=""grouplevel0_summary"" type=""xs:string""/>
											<xs:element name=""grouplevel1_summary"" type=""xs:string""/>
											<xs:element name=""grouplevel2_summary"" type=""xs:string"" minOccurs=""0""/>
										</xs:sequence>
									</xs:complexType>
								</xs:element>
								<xs:element name=""summary_band2_summary"" msprop:ProxyVersion=""v2"" msprop:viewpane=""summary"">
									<xs:complexType>
										<xs:sequence>
											<xs:element name=""corporate"" msprop:dbcolumn=""corporate"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""creditparty"" msprop:dbtable="""" msprop:summary_x003A_drill=""True"" msprop:summary_x003A_sortseq=""1"" msprop:summary_x003A_sortorder=""ASC"" msprop:expression="""" msprop:dbcolumn=""creditparty"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""description"" msprop:dbcolumn=""description"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""limit"" msprop:summary_x003A_subtotal=""True"" msprop:dbcolumn=""limit"" msprop:summary_x003A_columnformat=""#,##0;(#,##0)"" msprop:expression="""" msprop:dbtable="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""collateral"" msprop:summary_x003A_subtotal=""True"" msprop:dbcolumn=""collateral"" msprop:summary_x003A_columnformat=""#,##0;(#,##0)"" msprop:expression="""" msprop:dbtable="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""openinvoice"" msprop:summary_x003A_subtotal=""True"" msprop:dbcolumn=""openinvoice"" msprop:summary_x003A_columnformat=""#,##0;(#,##0)"" msprop:expression="""" msprop:dbtable="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""current"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""forward"" msprop:summary_x003A_subtotal=""True"" msprop:dbcolumn=""forward"" msprop:summary_x003A_columnformat=""#,##0;(#,##0)"" msprop:expression="""" msprop:dbtable="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""exposure"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""contract"" msprop:dbtable="""" msprop:summary_x003A_drill=""True"" msprop:summary_x003A_sortseq=""2"" msprop:summary_x003A_sortorder=""ASC"" msprop:expression="""" msprop:dbcolumn=""contract"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""credittype"" msprop:dbtable="""" msprop:summary_x003A_drill=""True"" msprop:summary_x003A_sortseq=""3"" msprop:summary_x003A_sortorder=""ASC"" msprop:expression="""" msprop:dbcolumn=""credittype"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""currentexposure"" msprop:summary_x003A_subtotal=""True"" msprop:dbcolumn=""currentexposure"" msprop:summary_x003A_columnformat=""#,##0;(#,##0)"" msprop:expression="""" msprop:dbtable="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""totalexposure"" msprop:summary_x003A_subtotal=""True"" msprop:dbcolumn=""totalexposure"" msprop:summary_x003A_columnformat=""#,##0;(#,##0)"" msprop:expression="""" msprop:dbtable="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""currency"" msprop:dbcolumn=""currency"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""begtime"" msdata:DateTimeMode=""Unspecified"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""endtime"" msdata:DateTimeMode=""Unspecified"" type=""xs:dateTime"" minOccurs=""0""/>
											<xs:element name=""company"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""m0"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""groupdesc"" msprop:dbcolumn=""groupdesc"" msprop:expression="""" msprop:dbtable="""" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""groupstatus"" type=""xs:int"" minOccurs=""0""/>
											<xs:element name=""grouplevel"" type=""xs:int"" minOccurs=""0""/>
											<xs:element name=""grouptotal"" type=""xs:int"" minOccurs=""0""/>
											<xs:element name=""counterparty"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""creditdetailreference"" type=""xs:string"" minOccurs=""0""/>
											<xs:element name=""creditexposure"" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""deliveredexposure"" msprop:summary_x003A_subtotal=""True"" msprop:dbcolumn="""" msprop:summary_x003A_columnformat=""#,##0;(#,##0)"" msprop:expression="""" msprop:dbtable="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""availablecredit"" msprop:summary_x003A_subtotal=""True"" msprop:dbcolumn="""" msprop:summary_x003A_columnformat=""#,##0;(#,##0)"" msprop:expression="""" msprop:dbtable="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""m"" msprop:summary_x003A_subtotal=""True"" msprop:dbcolumn=""m"" msprop:summary_x003A_columnformat=""#,##0;(#,##0)"" msprop:expression="""" msprop:dbtable="""" type=""xs:decimal"" minOccurs=""0""/>
											<xs:element name=""currencyfactor"" msdata:Caption=""Currency factor"" msprop:dbcolumn=""currencyfactor"" msprop:summary_x003A_columnformat=""#,##0"" msprop:expression="""" msprop:dbtable=""valuationdetail"" type=""xs:decimal"" default=""1"" minOccurs=""0""/>
											<xs:element name=""RowNumber"" type=""xs:int"" minOccurs=""0""/>
											<xs:element name=""grouplevel0_summary"" type=""xs:string""/>
											<xs:element name=""grouplevel1_summary"" type=""xs:string""/>
											<xs:element name=""grouplevel2_summary"" type=""xs:string""/>
										</xs:sequence>
									</xs:complexType>
								</xs:element>
							</xs:choice>
						</xs:complexType>
						<xs:unique name=""Constraint1"" msdata:PrimaryKey=""true"">
							<xs:selector xpath="".//mstns:trade""/>
							<xs:field xpath=""mstns:trade""/>
						</xs:unique>
						<xs:unique name=""Constraint2"">
							<xs:selector xpath="".//mstns:trade""/>
							<xs:field xpath=""mstns:trade""/>
							<xs:field xpath=""mstns:timezone""/>
						</xs:unique>
						<xs:unique name=""Constraint3"">
							<xs:selector xpath="".//mstns:trade""/>
							<xs:field xpath=""mstns:collaboration""/>
						</xs:unique>
						<xs:unique name=""positionquality_Constraint1"" msdata:ConstraintName=""Constraint1"" msdata:PrimaryKey=""true"">
							<xs:selector xpath="".//mstns:positionquality""/>
							<xs:field xpath=""mstns:fee""/>
						</xs:unique>
						<xs:unique name=""positionconstraint_Constraint1"" msdata:ConstraintName=""Constraint1"" msdata:PrimaryKey=""true"">
							<xs:selector xpath="".//mstns:positionconstraint""/>
							<xs:field xpath=""mstns:position""/>
							<xs:field xpath=""mstns:surrogate""/>
						</xs:unique>
						<xs:unique name=""tradeforwardcurve_Constraint1"" msdata:ConstraintName=""Constraint1"" msdata:PrimaryKey=""true"">
							<xs:selector xpath="".//mstns:tradeforwardcurve""/>
							<xs:field xpath=""mstns:position""/>
							<xs:field xpath=""mstns:trade""/>
							<xs:field xpath=""mstns:surrogate""/>
						</xs:unique>
						<xs:unique name=""tradeexercise_Constraint1"" msdata:ConstraintName=""Constraint1"" msdata:PrimaryKey=""true"">
							<xs:selector xpath="".//mstns:tradeexercise""/>
							<xs:field xpath=""mstns:position""/>
							<xs:field xpath=""mstns:surrogate""/>
						</xs:unique>
						<xs:unique name=""valuationdetail_Constraint1"" msdata:ConstraintName=""Constraint1"" msdata:PrimaryKey=""true"">
							<xs:selector xpath="".//mstns:valuationdetail""/>
							<xs:field xpath=""mstns:valuationdetail""/>
						</xs:unique>
						<xs:unique name=""tradedetailKey1"" msdata:PrimaryKey=""true"">
							<xs:selector xpath="".//mstns:tradedetail""/>
							<xs:field xpath=""mstns:position""/>
							<xs:field xpath=""mstns:posdetail""/>
						</xs:unique>
						<xs:unique name=""tradedetail_Constraint1"" msdata:ConstraintName=""Constraint1"">
							<xs:selector xpath="".//mstns:tradedetail""/>
							<xs:field xpath=""mstns:position""/>
						</xs:unique>
						<xs:unique name=""tradedetail_Constraint2"" msdata:ConstraintName=""Constraint2"">
							<xs:selector xpath="".//mstns:tradedetail""/>
							<xs:field xpath=""mstns:position""/>
							<xs:field xpath=""mstns:trade""/>
						</xs:unique>
						<xs:unique name=""tradedetail_Constraint3"" msdata:ConstraintName=""Constraint3"">
							<xs:selector xpath="".//mstns:tradedetail""/>
							<xs:field xpath=""mstns:position""/>
							<xs:field xpath=""mstns:feecode""/>
						</xs:unique>
						<xs:unique name=""quantitydetailKey1"" msdata:PrimaryKey=""true"">
							<xs:selector xpath="".//mstns:quantitydetail""/>
							<xs:field xpath=""mstns:position""/>
							<xs:field xpath=""mstns:posdetail""/>
							<xs:field xpath=""mstns:surrogate""/>
						</xs:unique>
						<xs:unique name=""feedetailKey1"" msdata:PrimaryKey=""true"">
							<xs:selector xpath="".//mstns:feedetail""/>
							<xs:field xpath=""mstns:fee""/>
						</xs:unique>
						<xs:unique name=""feedetail_Constraint2"" msdata:ConstraintName=""Constraint2"">
							<xs:selector xpath="".//mstns:feedetail""/>
							<xs:field xpath=""mstns:dbvalue""/>
							<xs:field xpath=""mstns:fee""/>
						</xs:unique>
						<xs:unique name=""pricedetail_Constraint1"" msdata:ConstraintName=""Constraint1"" msdata:PrimaryKey=""true"">
							<xs:selector xpath="".//mstns:pricedetail""/>
							<xs:field xpath=""mstns:fee""/>
						</xs:unique>
						<xs:unique name=""pricedetail_Constraint2"" msdata:ConstraintName=""Constraint2"">
							<xs:selector xpath="".//mstns:pricedetail""/>
							<xs:field xpath=""mstns:dbvalue""/>
							<xs:field xpath=""mstns:fee""/>
						</xs:unique>
						<xs:unique name=""positioncomponent_Constraint1"" msdata:ConstraintName=""Constraint1"" msdata:PrimaryKey=""true"">
							<xs:selector xpath="".//mstns:positioncomponent""/>
							<xs:field xpath=""mstns:position""/>
							<xs:field xpath=""mstns:begtime""/>
							<xs:field xpath=""mstns:endtime""/>
							<xs:field xpath=""mstns:component""/>
						</xs:unique>
						<xs:unique name=""optiondetail_Constraint1"" msdata:ConstraintName=""Constraint1"" msdata:PrimaryKey=""true"">
							<xs:selector xpath="".//mstns:optiondetail""/>
							<xs:field xpath=""mstns:position""/>
							<xs:field xpath=""mstns:surrogate""/>
						</xs:unique>
						<xs:unique name=""triggerprice_Constraint1"" msdata:ConstraintName=""Constraint1"" msdata:PrimaryKey=""true"">
							<xs:selector xpath="".//mstns:triggerprice""/>
							<xs:field xpath=""mstns:surrogate""/>
							<xs:field xpath=""mstns:position""/>
						</xs:unique>
						<xs:unique name=""verticalquantitydetail_Constraint1"" msdata:ConstraintName=""Constraint1"" msdata:PrimaryKey=""true"">
							<xs:selector xpath="".//mstns:verticalquantitydetail""/>
							<xs:field xpath=""mstns:position""/>
							<xs:field xpath=""mstns:posdetail""/>
							<xs:field xpath=""mstns:surrogate""/>
						</xs:unique>
						<xs:unique name=""pretradecreditexposure_Constraint1"" msdata:ConstraintName=""Constraint1"" msdata:PrimaryKey=""true"">
							<xs:selector xpath="".//mstns:pretradecreditexposure""/>
							<xs:field xpath=""mstns:pretradecreditexposureid""/>
						</xs:unique>
						<xs:unique name=""pretradeforwardcreditexposure_Constraint1"" msdata:ConstraintName=""Constraint1"" msdata:PrimaryKey=""true"">
							<xs:selector xpath="".//mstns:pretradeforwardcreditexposure""/>
							<xs:field xpath=""mstns:pretradeforwardcreditexposureid""/>
						</xs:unique>
						<xs:unique name=""pretradecreditcheckmatrix_Constraint1"" msdata:ConstraintName=""Constraint1"" msdata:PrimaryKey=""true"">
							<xs:selector xpath="".//mstns:pretradecreditcheckmatrix""/>
							<xs:field xpath=""mstns:pretradecreditcheckmatrixid""/>
						</xs:unique>
						<xs:unique name=""pretradeforwardcreditcheckmatrix_Constraint1"" msdata:ConstraintName=""Constraint1"" msdata:PrimaryKey=""true"">
							<xs:selector xpath="".//mstns:pretradeforwardcreditcheckmatrix""/>
							<xs:field xpath=""mstns:pretradeforwardcreditcheckmatrixid""/>
						</xs:unique>
						<xs:unique name=""trade_approval_Constraint1"" msdata:ConstraintName=""Constraint1"" msdata:PrimaryKey=""true"">
							<xs:selector xpath="".//mstns:trade_approval""/>
							<xs:field xpath=""mstns:collaboration""/>
							<xs:field xpath=""mstns:approvaltype""/>
						</xs:unique>
						<xs:unique name=""trade_calendar_Constraint1"" msdata:ConstraintName=""Constraint1"" msdata:PrimaryKey=""true"">
							<xs:selector xpath="".//mstns:trade_calendar""/>
							<xs:field xpath=""mstns:collaboration""/>
							<xs:field xpath=""mstns:surrogate""/>
						</xs:unique>
						<xs:unique name=""trade_correspondence_Constraint1"" msdata:ConstraintName=""Constraint1"" msdata:PrimaryKey=""true"">
							<xs:selector xpath="".//mstns:trade_correspondence""/>
							<xs:field xpath=""mstns:collaboration""/>
							<xs:field xpath=""mstns:correspondencedate""/>
						</xs:unique>
						<xs:unique name=""trade_document_Constraint1"" msdata:ConstraintName=""Constraint1"" msdata:PrimaryKey=""true"">
							<xs:selector xpath="".//mstns:trade_document""/>
							<xs:field xpath=""mstns:collaboration""/>
							<xs:field xpath=""mstns:surrogate""/>
						</xs:unique>
						<xs:unique name=""trade_interface_Constraint1"" msdata:ConstraintName=""Constraint1"" msdata:PrimaryKey=""true"">
							<xs:selector xpath="".//mstns:trade_interface""/>
							<xs:field xpath=""mstns:collaboration""/>
							<xs:field xpath=""mstns:interfaceclass""/>
						</xs:unique>
						<xs:unique name=""trade_remark_Constraint1"" msdata:ConstraintName=""Constraint1"" msdata:PrimaryKey=""true"">
							<xs:selector xpath="".//mstns:trade_remark""/>
							<xs:field xpath=""mstns:collaboration""/>
							<xs:field xpath=""mstns:surrogate""/>
						</xs:unique>
						<xs:unique name=""trade_workflowstate_Constraint1"" msdata:ConstraintName=""Constraint1"" msdata:PrimaryKey=""true"">
							<xs:selector xpath="".//mstns:trade_workflowstate""/>
							<xs:field xpath=""mstns:collaboration""/>
							<xs:field xpath=""mstns:taskid""/>
						</xs:unique>
						<xs:unique name=""trade_workflowstate_Constraint3"" msdata:ConstraintName=""Constraint3"">
							<xs:selector xpath="".//mstns:trade_workflowstate""/>
							<xs:field xpath=""mstns:collaboration""/>
							<xs:field xpath=""mstns:alternateflow""/>
						</xs:unique>
						<xs:unique name=""summary_band0_summary_Constraint1"" msdata:ConstraintName=""Constraint1"" msdata:PrimaryKey=""true"">
							<xs:selector xpath="".//mstns:summary_band0_summary""/>
							<xs:field xpath=""mstns:grouplevel0_summary""/>
						</xs:unique>
						<xs:unique name=""summary_band1_summary_Constraint1"" msdata:ConstraintName=""Constraint1"" msdata:PrimaryKey=""true"">
							<xs:selector xpath="".//mstns:summary_band1_summary""/>
							<xs:field xpath=""mstns:grouplevel0_summary""/>
							<xs:field xpath=""mstns:grouplevel1_summary""/>
						</xs:unique>
						<xs:unique name=""summary_band2_summary_Constraint1"" msdata:ConstraintName=""Constraint1"" msdata:PrimaryKey=""true"">
							<xs:selector xpath="".//mstns:summary_band2_summary""/>
							<xs:field xpath=""mstns:grouplevel0_summary""/>
							<xs:field xpath=""mstns:grouplevel1_summary""/>
							<xs:field xpath=""mstns:grouplevel2_summary""/>
						</xs:unique>
						<xs:keyref name=""summary_band1_band2_summary"" refer=""summary_band1_summary_Constraint1"">
							<xs:selector xpath="".//mstns:summary_band2_summary""/>
							<xs:field xpath=""mstns:grouplevel0_summary""/>
							<xs:field xpath=""mstns:grouplevel1_summary""/>
						</xs:keyref>
						<xs:keyref name=""summary_band0_band1_summary"" refer=""summary_band0_summary_Constraint1"">
							<xs:selector xpath="".//mstns:summary_band1_summary""/>
							<xs:field xpath=""mstns:grouplevel0_summary""/>
						</xs:keyref>
						<xs:keyref name=""trade_workflowstate"" refer=""Constraint3"" msprop:rel_CascadeDuplicate=""False"">
							<xs:selector xpath="".//mstns:trade_workflowstate""/>
							<xs:field xpath=""mstns:collaboration""/>
						</xs:keyref>
						<xs:keyref name=""trade_workflowstate_alternateflow"" refer=""trade_workflowstate_Constraint3"" msprop:rel_CascadeDuplicate=""False"">
							<xs:selector xpath="".//mstns:trade_workflowstate""/>
							<xs:field xpath=""mstns:collaboration""/>
							<xs:field xpath=""mstns:workflowid""/>
						</xs:keyref>
						<xs:keyref name=""trade_remark"" refer=""Constraint3"" msprop:rel_CascadeDuplicate=""False"">
							<xs:selector xpath="".//mstns:trade_remark""/>
							<xs:field xpath=""mstns:collaboration""/>
						</xs:keyref>
						<xs:keyref name=""trade_interface"" refer=""Constraint3"" msprop:rel_CascadeDuplicate=""False"">
							<xs:selector xpath="".//mstns:trade_interface""/>
							<xs:field xpath=""mstns:collaboration""/>
						</xs:keyref>
						<xs:keyref name=""trade_document"" refer=""Constraint3"" msprop:rel_CascadeDuplicate=""False"">
							<xs:selector xpath="".//mstns:trade_document""/>
							<xs:field xpath=""mstns:collaboration""/>
						</xs:keyref>
						<xs:keyref name=""trade_correspondence"" refer=""Constraint3"" msprop:rel_CascadeDuplicate=""False"">
							<xs:selector xpath="".//mstns:trade_correspondence""/>
							<xs:field xpath=""mstns:collaboration""/>
						</xs:keyref>
						<xs:keyref name=""trade_calendar"" refer=""Constraint3"" msprop:rel_CascadeDuplicate=""False"">
							<xs:selector xpath="".//mstns:trade_calendar""/>
							<xs:field xpath=""mstns:collaboration""/>
						</xs:keyref>
						<xs:keyref name=""trade_approval"" refer=""Constraint3"" msprop:rel_CascadeDuplicate=""False"">
							<xs:selector xpath="".//mstns:trade_approval""/>
							<xs:field xpath=""mstns:collaboration""/>
						</xs:keyref>
						<xs:keyref name=""tradedetail_verticalquantitydetail"" refer=""tradedetailKey1"">
							<xs:selector xpath="".//mstns:verticalquantitydetail""/>
							<xs:field xpath=""mstns:position""/>
							<xs:field xpath=""mstns:posdetail""/>
						</xs:keyref>
						<xs:keyref name=""tradedetail_triggerprice"" refer=""tradedetail_Constraint1"">
							<xs:selector xpath="".//mstns:triggerprice""/>
							<xs:field xpath=""mstns:position""/>
						</xs:keyref>
						<xs:keyref name=""tradedetail_optiondetail"" refer=""tradedetail_Constraint1"" msdata:DeleteRule=""None"">
							<xs:selector xpath="".//mstns:optiondetail""/>
							<xs:field xpath=""mstns:position""/>
						</xs:keyref>
						<xs:keyref name=""tradedetail_positioncomponent"" refer=""tradedetail_Constraint1"" msdata:DeleteRule=""None"">
							<xs:selector xpath="".//mstns:positioncomponent""/>
							<xs:field xpath=""mstns:position""/>
						</xs:keyref>
						<xs:keyref name=""pricedetail_Constraint3"" refer=""tradedetail_Constraint3"" msdata:ConstraintName=""Constraint3"" msdata:RelationName=""tradedetail_pricedetail"">
							<xs:selector xpath="".//mstns:pricedetail""/>
							<xs:field xpath=""mstns:dbvalue""/>
							<xs:field xpath=""mstns:feecode""/>
						</xs:keyref>
						<xs:keyref name=""tradedetail_pricedetail"" refer=""tradedetail_Constraint1"" msdata:ConstraintOnly=""true"" msdata:DeleteRule=""None"">
							<xs:selector xpath="".//mstns:pricedetail""/>
							<xs:field xpath=""mstns:dbvalue""/>
						</xs:keyref>
						<xs:keyref name=""pricedetail_pricedetail"" refer=""pricedetail_Constraint2"">
							<xs:selector xpath="".//mstns:pricedetail""/>
							<xs:field xpath=""mstns:dbvalue""/>
							<xs:field xpath=""mstns:parentfee""/>
						</xs:keyref>
						<xs:keyref name=""feedetail_Constraint1"" refer=""tradedetail_Constraint3"" msdata:ConstraintName=""Constraint1"" msdata:RelationName=""tradedetail_feedetail"">
							<xs:selector xpath="".//mstns:feedetail""/>
							<xs:field xpath=""mstns:dbvalue""/>
							<xs:field xpath=""mstns:feecode""/>
						</xs:keyref>
						<xs:keyref name=""tradedetail_feedetail"" refer=""tradedetail_Constraint1"" msdata:ConstraintOnly=""true"" msdata:DeleteRule=""None"">
							<xs:selector xpath="".//mstns:feedetail""/>
							<xs:field xpath=""mstns:dbvalue""/>
						</xs:keyref>
						<xs:keyref name=""feedetail_feedetail"" refer=""feedetail_Constraint2"">
							<xs:selector xpath="".//mstns:feedetail""/>
							<xs:field xpath=""mstns:dbvalue""/>
							<xs:field xpath=""mstns:parentfee""/>
						</xs:keyref>
						<xs:keyref name=""tradedetail_quantitydetail"" refer=""tradedetailKey1"">
							<xs:selector xpath="".//mstns:quantitydetail""/>
							<xs:field xpath=""mstns:position""/>
							<xs:field xpath=""mstns:posdetail""/>
						</xs:keyref>
						<xs:keyref name=""fkc_trade_tradedetail_timezone"" refer=""Constraint2"" msdata:ConstraintOnly=""true"" msdata:DeleteRule=""None"">
							<xs:selector xpath="".//mstns:tradedetail""/>
							<xs:field xpath=""mstns:trade""/>
							<xs:field xpath=""mstns:timezone""/>
						</xs:keyref>
						<xs:keyref name=""trade_tradedetail"" refer=""Constraint1"">
							<xs:selector xpath="".//mstns:tradedetail""/>
							<xs:field xpath=""mstns:trade""/>
						</xs:keyref>
						<xs:keyref name=""summary_band2_band3_summary"" refer=""summary_band2_summary_Constraint1"">
							<xs:selector xpath="".//mstns:summary""/>
							<xs:field xpath=""mstns:grouplevel0_summary""/>
							<xs:field xpath=""mstns:grouplevel1_summary""/>
							<xs:field xpath=""mstns:grouplevel2_summary""/>
						</xs:keyref>
						<xs:keyref name=""trade_valuationdetail"" refer=""Constraint1"" msprop:rel_CascadeDuplicate=""False"">
							<xs:selector xpath="".//mstns:valuationdetail""/>
							<xs:field xpath=""mstns:trade""/>
						</xs:keyref>
						<xs:keyref name=""tradedetail_tradeexercise"" refer=""tradedetail_Constraint2"" msprop:rel_CascadeDuplicate=""False"" msdata:DeleteRule=""None"">
							<xs:selector xpath="".//mstns:tradeexercise""/>
							<xs:field xpath=""mstns:position""/>
							<xs:field xpath=""mstns:trade""/>
						</xs:keyref>
						<xs:keyref name=""tradedetail_tradeforwardcurve"" refer=""tradedetail_Constraint2"" msdata:DeleteRule=""None"">
							<xs:selector xpath="".//mstns:tradeforwardcurve""/>
							<xs:field xpath=""mstns:position""/>
							<xs:field xpath=""mstns:trade""/>
						</xs:keyref>
						<xs:keyref name=""tradedetail_positionconstraint"" refer=""tradedetail_Constraint1"" msdata:DeleteRule=""None"">
							<xs:selector xpath="".//mstns:positionconstraint""/>
							<xs:field xpath=""mstns:position""/>
						</xs:keyref>
						<xs:keyref name=""tradedetail_positionquality"" refer=""tradedetail_Constraint1"" msdata:DeleteRule=""None"">
							<xs:selector xpath="".//mstns:positionquality""/>
							<xs:field xpath=""mstns:position""/>
						</xs:keyref>
					</xs:element>
				</xs:schema>
				<diffgr:diffgram xmlns:msdata=""urn:schemas-microsoft-com:xml-msdata"" xmlns:diffgr=""urn:schemas-microsoft-com:xml-diffgram-v1"">
					<TradeExecutionUIDS xmlns=""http://allegrodevelopment.com/TradeExecutionUIDS.xsd"">
						<trade diffgr:id=""trade1"" msdata:rowOrder=""0"" >
							<trade>100006</trade>
							<tradetype>Physical Liquid</tradetype>
							<tradedate>2024-11-18T07:45:40</tradedate>
							<company>Vista Upstream Inc.</company>
							<begtime>2024-12-01T00:00:00</begtime>
							<endtime>2026-12-31T00:00:00</endtime>
							<tradestatus>New</tradestatus>
							<contractprep>true</contractprep>
							<timeseries>false</timeseries>
							<status>ACTIVE</status>
							<confirmstatus>NEW</confirmstatus>
							<tradeclass>Oil Upstrram</tradeclass>
							<trader>Julio Trujillo</trader>
							<settlementfreq>MONTH</settlementfreq>
							<currency>USD</currency>
							<taxable>false</taxable>
							<evergreen>false</evergreen>
							<internal>false</internal>
							<internaltrade/>
							<begdate>2024-12-01T00:00:00</begdate>
							<enddate>2026-12-31T00:00:00</enddate>
							<physicalexchange>false</physicalexchange>
							<titletransfer>false</titletransfer>
							<executetrade>true</executetrade>
							<evergreenforecast>0</evergreenforecast>
							<fallback>false</fallback>
							<document>false</document>
							<calendar>false</calendar>
							<correspondence>false</correspondence>
							<remark>false</remark>
							<collaboration>100037</collaboration>
							<creationname>Julio Trujillo</creationname>
							<creationdate>2024-11-18T10:04:26.2413896</creationdate>
							<product>Medanito</product>
							<positionmode>PHYSICAL</positionmode>
							<h_version>1</h_version>
							<positiontype>SELL</positiontype>
							<counterparty>VISTA</counterparty>
							<contract>100002</contract>
							<broker_contract>100002</broker_contract>
							<tradebook>Oil Medanito</tradebook>
							<unit>bbl</unit>
							<timeunit>TOTAL</timeunit>
							<fee_pricediff>0</fee_pricediff>
							<fee_pricelevel>AVG</fee_pricelevel>
							<pricetype>FIXED</pricetype>
							<paymentterms>10 days after delivery</paymentterms>
							<servicestatus>PRIMARY</servicestatus>
							<segment>false</segment>
							<secondary>false</secondary>
							<firm>true</firm>
							<ng_decint>1</ng_decint>
							<quantitymethod>DELIVERY</quantitymethod>
							<phys_product>Medanito</phys_product>
							<power_decint>1</power_decint>
							<forwardquantity>false</forwardquantity>
							<phys_decint>1</phys_decint>
							<paycongestion>false</paycongestion>
							<preconfirmed>false</preconfirmed>
							<genpct>1</genpct>
							<valuedate>2024-11-18T00:00:00</valuedate>
							<rate>0</rate>
							<fee_currency>USD</fee_currency>
							<strikeprice>0</strikeprice>
							<premium>0</premium>
							<premiummethod>VARIABLE</premiummethod>
							<optionfrequency>MONTH</optionfrequency>
							<optionposition>false</optionposition>
							<production>false</production>
							<consumption>false</consumption>
							<pay_currency>USD</pay_currency>
							<pay_pricediff>0</pay_pricediff>
							<rec_currency>USD</rec_currency>
							<rec_pricediff>0</rec_pricediff>
							<strike_currency>USD</strike_currency>
							<strike_pricediff>0</strike_pricediff>
							<reference_currency>USD</reference_currency>
							<reference_pricediff>0</reference_pricediff>
							<heatrate>1</heatrate>
							<decint>1</decint>
							<trade_h_version>1</trade_h_version>
							<position_h_version>1</position_h_version>
							<RowNumber>0</RowNumber>
						</trade>
						<tradedetail diffgr:id=""tradedetail2"" msdata:rowOrder=""1"" diffgr:hasChanges=""inserted"">
							<position>001</position>
							<contract>100002</contract>
							<trade>100006</trade>
							<positionmode>PHYSICAL</positionmode>
							<positiontype>SELL</positiontype>
							<timeseries>false</timeseries>
							<counterparty>VISTA</counterparty>
							<unit>bbl</unit>
							<company>Vista Upstream Inc.</company>
							<product>Medanito</product>
							<tradebook>Oil Medanito</tradebook>
							<paymentterms>10 days after delivery</paymentterms>
							<optionposition>false</optionposition>
							<premium>0</premium>
							<premiummethod>VARIABLE</premiummethod>
							<optionfrequency>MONTH</optionfrequency>
							<settlementfrequency>MONTH</settlementfrequency>
							<production>false</production>
							<consumption>false</consumption>
							<strikeprice>0</strikeprice>
							<collaboration>001</collaboration>
							<positiondbtable>physicalposition</positiondbtable>
							<posdetail>001</posdetail>
							<begtime>2025-03-01T00:00:00</begtime>
							<endtime>2025-04-01T00:00:00</endtime>
							<quantity>1500</quantity>
							<timeunit>TOTAL</timeunit>
							<valuedate>2024-11-18T00:00:00</valuedate>
							<rate>0</rate>
							<servicestatus>PRIMARY</servicestatus>
							<segment>false</segment>
							<secondary>false</secondary>
							<firm>true</firm>
							<decint>1</decint>
							<quantitymethod>DELIVERY</quantitymethod>
							<oba>false</oba>
							<paycongestion>false</paycongestion>
							<preconfirmed>false</preconfirmed>
							<genpct>1</genpct>
							<forwardquantity>false</forwardquantity>
							<alternate>false</alternate>
							<quantitytype>DELIVERY</quantitytype>
							<quantitystatus>TRADE</quantitystatus>
							<posstatus>false</posstatus>
							<mass>849</mass>
							<massunit>mt</massunit>
							<volume>1000</volume>
							<volumeunit>bbl</volumeunit>
							<gravity>35.10000000</gravity>
							<gravityunit>API</gravityunit>
							<altquantity>0</altquantity>
							<ofo>false</ofo>
							<qualityspec>false</qualityspec>
							<daylightsaving>false</daylightsaving>
							<cutstatus>false</cutstatus>
							<tradeoption>false</tradeoption>
							<exerciseinstrument>CASH SETTLED</exerciseinstrument>
							<voltype>FLAT</voltype>
							<forwardtype>FLAT</forwardtype>
							<discounttype>FLAT</discounttype>
							<position_h_version>1</position_h_version>
							<prodposition_h_version>1</prodposition_h_version>
							<prodquantity_h_version>1</prodquantity_h_version>
							<usestorage>false</usestorage>
							<totalquantity>1000</totalquantity>
							<fee_indexprecision>5</fee_indexprecision>
							<fee_priceprecision>5</fee_priceprecision>
							<fee_pricediff>0</fee_pricediff>
							<pricelevel>AVG</pricelevel>
							<swingtolerance>0</swingtolerance>
							<entryexitbalancetype>PHYSICAL</entryexitbalancetype>
							<entryexitcapacityrequired>false</entryexitcapacityrequired>
							<strike_pricediff>0</strike_pricediff>
							<evergreenforecast>0</evergreenforecast>
							<fee_currency>USD</fee_currency>
							<fee_feemethod>COMMODITY PRICE</fee_feemethod>
							<constrainttimeunit>MONTH</constrainttimeunit>
							<cascadable>false</cascadable>
							<settlephysically>false</settlephysically>
							<heatrate>1</heatrate>
							<ldcfirmquantity>0</ldcfirmquantity>
							<useshipperstorage>false</useshipperstorage>
							<demurragecurrency>USD</demurragecurrency>
							<finposition_h_version>1</finposition_h_version>
							<RowNumber>1</RowNumber>
						</tradedetail>
					</TradeExecutionUIDS>
				</diffgr:diffgram>
			</tradeExecutionDS>
			<criteria>
				<DateColumn>tradedate</DateColumn>
				<BegTime>2024-11-01T00:00:00</BegTime>
				<EndTime>2024-12-01T00:00:00</EndTime>
				<FilterByRelation>true</FilterByRelation>
				<FilterDateByRelation>false</FilterDateByRelation>
				<FilterByForeignKey>false</FilterByForeignKey>
				<DbCriteria>
					<DbCriteria>
						<LogOperator>AND</LogOperator>
						<OpenParen/>
						<DbTable>trade</DbTable>
						<DbColumn>status</DbColumn>
						<RelOperator>=</RelOperator>
						<DbValue>ACTIVE</DbValue>
						<CloseParen/>
						<DbValueListString/>
						<DbValueListInt/>
					</DbCriteria>
				</DbCriteria>
			</criteria>
			<currency>ORIGINAL</currency>
			<timeunit>MONTH</timeunit>
			<creditvaluationmode/>
			<creditmode/>
			<pretradeforwarddays>180</pretradeforwarddays>
		</UpdateTradeExecution>
	</soap12:Body>
</soap12:Envelope>";

        return soapMessage;
    }
}
